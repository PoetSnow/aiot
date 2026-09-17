using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using NXAI.Device.Application.Contracts.Dtos;
using NXAI.Device.Application.Contracts.Interfaces;
using NXAI.Shared.Pot.Json;
using NXAI.Shared.Pot.Mqtt;

namespace NXAI.Device.Application.Mqtt;

/// <summary>连接 EMQX：下发 Outbox，解析上行 ACK/影子/能力。Broker 挂了不拖垮 Host。</summary>
public sealed class DeviceMqttHostedService(
    IServiceScopeFactory scopes,
    IConfiguration configuration,
    ILogger<DeviceMqttHostedService> logger) : IHostedService
{
    private CancellationTokenSource? _cts;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        _ = ExecuteAsync(_cts.Token);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _cts?.Cancel();
        return Task.CompletedTask;
    }

    private async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
        var options = new DeviceMqttOptions
        {
            Enabled = !string.Equals(configuration["Mqtt:Enabled"], "false", StringComparison.OrdinalIgnoreCase),
            Host = configuration["Mqtt:Host"] ?? "127.0.0.1",
            Port = int.TryParse(configuration["Mqtt:Port"], out var port) ? port : 1883,
            ClientId = configuration["Mqtt:ClientId"] ?? "nxai-device-svc",
            Username = configuration["Mqtt:Username"] ?? "nxai-device-svc",
            Password = configuration["Mqtt:Password"] ?? "nxai-device-svc"
        };
        if (!options.Enabled)
        {
            logger.LogInformation("MQTT worker disabled");
            return;
        }

        var factory = new MqttFactory();
        using var client = factory.CreateMqttClient();
        client.ApplicationMessageReceivedAsync += e => OnMessageAsync(e, stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                if (!client.IsConnected)
                {
                    var mqttOptions = new MqttClientOptionsBuilder()
                        .WithTcpServer(options.Host, options.Port)
                        .WithClientId(options.ClientId)
                        .WithCredentials(options.Username, options.Password)
                        .WithCleanSession()
                        .Build();
                    await client.ConnectAsync(mqttOptions, stoppingToken);
                    await client.SubscribeAsync("pot/+/up/#");
                    logger.LogInformation("MQTT connected {Host}:{Port}", options.Host, options.Port);
                }

                await DrainOutboxAsync(client, stoppingToken);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                logger.LogWarning(ex, "MQTT loop retry");
            }

            await Task.Delay(TimeSpan.FromMilliseconds(800), stoppingToken);
        }
        }
        catch (OperationCanceledException)
        {
        }
    }

    private async Task DrainOutboxAsync(IMqttClient client, CancellationToken ct)
    {
        if (!client.IsConnected)
        {
            return;
        }

        using var scope = scopes.CreateScope();
        var devices = scope.ServiceProvider.GetRequiredService<IDeviceService>();
        var pending = await devices.TakePendingOutboxAsync(20);
        foreach (var item in pending)
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic($"pot/{item.DeviceSn}/down/command")
                .WithPayload(item.PayloadJson)
                .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce)
                .Build();
            await client.PublishAsync(message, ct);
            await devices.MarkOutboxAttemptAsync(item.OutboxId);
        }
    }

    private async Task OnMessageAsync(MqttApplicationMessageReceivedEventArgs args, CancellationToken ct)
    {
        var topic = args.ApplicationMessage.Topic ?? string.Empty;
        var json = args.ApplicationMessage.ConvertPayloadToString();
        if (string.IsNullOrWhiteSpace(json))
        {
            return;
        }

        var parts = topic.Split('/');
        if (parts.Length < 4 || !string.Equals(parts[0], "pot", StringComparison.Ordinal) || !string.Equals(parts[2], "up", StringComparison.Ordinal))
        {
            return;
        }

        var sn = parts[1];
        var kind = parts[3];
        using var scope = scopes.CreateScope();
        var devices = scope.ServiceProvider.GetRequiredService<IDeviceService>();
        var telemetry = scope.ServiceProvider.GetRequiredService<ITelemetryStore>();

        try
        {
            if (string.Equals(kind, "ack", StringComparison.OrdinalIgnoreCase))
            {
                var envelope = PotJson.Deserialize<PotMqttEnvelope<PotAckPayload>>(json);
                var payload = envelope?.Payload;
                if (payload is null)
                {
                    return;
                }

                await devices.ApplyAckAsync(sn, new DeviceAckDto
                {
                    CommandId = payload.CommandId,
                    TaskId = payload.TaskId,
                    Epoch = payload.Epoch,
                    StepNo = payload.StepNo,
                    Result = payload.Result.ToString(),
                    WaterTemp = payload.WaterTemp
                });
                return;
            }

            if (string.Equals(kind, "capability", StringComparison.OrdinalIgnoreCase))
            {
                using var doc = JsonDocument.Parse(json);
                var payload = doc.RootElement.TryGetProperty("payload", out var p) ? p.GetRawText() : json;
                await devices.ApplyCapabilityAsync(sn, payload);
                return;
            }

            if (string.Equals(kind, "shadow", StringComparison.OrdinalIgnoreCase))
            {
                using var doc = JsonDocument.Parse(json);
                var payload = doc.RootElement.TryGetProperty("payload", out var p) ? p : doc.RootElement;
                var report = new DeviceShadowReportDto();
                if (payload.TryGetProperty("version", out var version) && version.TryGetInt32(out var v))
                {
                    report.Version = v;
                }

                if (payload.TryGetProperty("online", out var online))
                {
                    report.Online = online.ValueKind == JsonValueKind.True || (online.ValueKind == JsonValueKind.String && bool.TryParse(online.GetString(), out var b) && b);
                }

                if (payload.TryGetProperty("waterTemp", out var temp) && temp.TryGetDecimal(out var t))
                {
                    report.WaterTemp = t;
                }

                if (payload.TryGetProperty("workState", out var state))
                {
                    report.WorkState = state.GetString();
                }

                if (payload.TryGetProperty("currentTaskId", out var taskEl) && taskEl.TryGetInt64(out var taskId))
                {
                    report.CurrentTaskId = taskId;
                }

                if (payload.TryGetProperty("currentEpoch", out var epochEl) && epochEl.TryGetInt32(out var epoch))
                {
                    report.CurrentEpoch = epoch;
                }

                if (payload.TryGetProperty("currentStepNo", out var stepEl) && stepEl.TryGetInt32(out var stepNo))
                {
                    report.CurrentStepNo = stepNo;
                }

                await devices.ApplyShadowAsync(sn, report);

                var ts = DateTime.UtcNow;
                var points = new List<TelemetryPointDto>();
                if (report.WaterTemp is not null)
                {
                    points.Add(new TelemetryPointDto(sn, "water_temp", ts, report.WaterTemp, report.CurrentTaskId, report.CurrentEpoch));
                }

                if (!string.IsNullOrWhiteSpace(report.WorkState))
                {
                    points.Add(new TelemetryPointDto(sn, "work_state", ts, null, report.CurrentTaskId, report.CurrentEpoch, report.WorkState));
                }

                if (report.Online is not null)
                {
                    points.Add(new TelemetryPointDto(sn, "online", ts, report.Online.Value ? 1 : 0, report.CurrentTaskId, report.CurrentEpoch, report.Online.Value.ToString()));
                }

                await telemetry.AppendAsync(points, ct);
            }
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "MQTT inbound {Topic} failed", topic);
        }
    }
}
