using System.Text.Json;
using MQTTnet;
using MQTTnet.Client;
using NXAI.Shared.Pot;
using NXAI.Shared.Pot.Enums;
using NXAI.Shared.Pot.Json;
using NXAI.Shared.Pot.Mqtt;

var broker = Arg(args, "--broker", "127.0.0.1");
var port = int.Parse(Arg(args, "--port", "1883"));
var sn = Arg(args, "--sn", "POT20260001");
var token = Arg(args, "--token", "");
var mode = Arg(args, "--mode", "happy");
var localTemp = decimal.Parse(Arg(args, "--local-temp", "80"));

var acks = new Dictionary<long, PotAckPayload>();
var currentEpoch = 0;
var expectedStepNo = 1;
var dispenseCount = 0;

var factory = new MqttFactory();
using var client = factory.CreateMqttClient();
client.ApplicationMessageReceivedAsync += async e =>
{
    var json = e.ApplicationMessage.ConvertPayloadToString();
    var envelope = PotJson.Deserialize<PotMqttEnvelope<PotCommandPayload>>(json);
    var cmd = envelope?.Payload;
    if (cmd is null)
    {
        return;
    }

    Console.WriteLine($"recv commandId={cmd.CommandId} action={cmd.Action} epoch={cmd.Epoch} step={cmd.StepNo}");

    if (acks.TryGetValue(cmd.CommandId, out var replay))
    {
        await PublishAck(client, sn, replay);
        Console.WriteLine($"replay commandId={cmd.CommandId} (dispenseCount={dispenseCount})");
        return;
    }

    var result = CommandResultCode.SUCCESS;
    var message = (string?)null;

    if (cmd.Epoch < currentEpoch)
    {
        result = CommandResultCode.REJECTED;
        message = "old epoch";
    }
    else if (cmd.Action == CommandAction.STOP && cmd.Epoch == currentEpoch)
    {
        expectedStepNo = 1;
        result = CommandResultCode.SUCCESS;
    }
    else
    {
        if (cmd.Epoch > currentEpoch)
        {
            currentEpoch = cmd.Epoch;
            expectedStepNo = cmd.StepNo;
        }

        if (cmd.StepNo != expectedStepNo)
        {
            result = CommandResultCode.REJECTED;
            message = "unexpected step";
        }
        else if (cmd.ExpireAt is long exp && exp < DateTimeOffset.UtcNow.ToUnixTimeSeconds())
        {
            result = CommandResultCode.REJECTED;
            message = "expired";
        }
        else if (cmd.Action == CommandAction.DISPENSE)
        {
            if (string.Equals(mode, "missing", StringComparison.OrdinalIgnoreCase))
            {
                result = CommandResultCode.MISSING;
                message = "slot empty";
            }
            else if (cmd.Trigger?.Type == TriggerType.TEMP_GTE && localTemp < (cmd.Trigger.TempCelsius ?? 80))
            {
                result = CommandResultCode.REJECTED;
                message = "local temp below trigger";
            }
            else
            {
                dispenseCount += 1;
                expectedStepNo = cmd.StepNo + 1;
            }
        }
        else if (cmd.Action == CommandAction.HEAT)
        {
            expectedStepNo = cmd.StepNo + 1;
        }
    }

    var ack = new PotAckPayload
    {
        CommandId = cmd.CommandId,
        TaskId = cmd.TaskId,
        Epoch = cmd.Epoch,
        StepNo = cmd.StepNo,
        Result = result,
        WaterTemp = localTemp,
        Message = message
    };
    acks[cmd.CommandId] = ack;

    await PublishAck(client, sn, new PotAckPayload { CommandId = cmd.CommandId, TaskId = cmd.TaskId, Epoch = cmd.Epoch, StepNo = cmd.StepNo, Result = CommandResultCode.RECEIVED, WaterTemp = localTemp });
    if (result != CommandResultCode.REJECTED)
    {
        await PublishAck(client, sn, new PotAckPayload { CommandId = cmd.CommandId, TaskId = cmd.TaskId, Epoch = cmd.Epoch, StepNo = cmd.StepNo, Result = CommandResultCode.RUNNING, WaterTemp = localTemp });
    }

    await PublishAck(client, sn, ack);
    Console.WriteLine($"ack {result} commandId={cmd.CommandId} dispenseCount={dispenseCount}");
};

var mqttOptions = new MqttClientOptionsBuilder()
    .WithTcpServer(broker, port)
    .WithClientId($"pot-{sn}")
    .WithCredentials(sn, string.IsNullOrWhiteSpace(token) ? "anonymous" : token)
    .WithCleanSession()
    .Build();

Console.WriteLine($"connecting {broker}:{port} as pot-{sn} mode={mode} localTemp={localTemp}");
await client.ConnectAsync(mqttOptions);
await client.SubscribeAsync($"pot/{sn}/down/#");

await PublishJson(client, $"pot/{sn}/up/capability", new
{
    schema = PotMqttSchemas.CmdV1,
    msgId = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
    ts = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
    sn,
    type = "CAPABILITY",
    payload = new
    {
        slots = new[]
        {
            new { slotCode = "S1", modes = new[] { "PACKAGE" } },
            new { slotCode = "S2", modes = new[] { "PACKAGE" } },
            new { slotCode = "S3", modes = new[] { "PACKAGE" } }
        },
        actions = new[] { "HEAT", "DISPENSE", "STOP" }
    }
});

await PublishJson(client, $"pot/{sn}/up/shadow", new
{
    schema = PotMqttSchemas.CmdV1,
    msgId = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
    ts = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
    sn,
    type = "SHADOW",
    payload = new
    {
        version = 1,
        online = true,
        waterTemp = localTemp,
        workState = "IDLE"
    }
});

Console.WriteLine("online. Ctrl+C to exit.");
await Task.Delay(Timeout.InfiniteTimeSpan);

static string Arg(string[] args, string name, string fallback)
{
    var index = Array.IndexOf(args, name);
    return index >= 0 && index + 1 < args.Length ? args[index + 1] : fallback;
}

static Task PublishAck(IMqttClient client, string sn, PotAckPayload payload)
{
    var envelope = new PotMqttEnvelope<PotAckPayload>
    {
        Schema = PotMqttSchemas.CmdV1,
        MsgId = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
        Ts = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
        Sn = sn,
        Type = PotMessageType.ACK,
        Payload = payload
    };
    return PublishRaw(client, $"pot/{sn}/up/ack", PotJson.Serialize(envelope));
}

static Task PublishJson(IMqttClient client, string topic, object payload)
    => PublishRaw(client, topic, JsonSerializer.Serialize(payload));

static Task PublishRaw(IMqttClient client, string topic, string json)
{
    var message = new MqttApplicationMessageBuilder()
        .WithTopic(topic)
        .WithPayload(json)
        .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce)
        .Build();
    return client.PublishAsync(message);
}
