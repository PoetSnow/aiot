using System.Text.Json;
using NXAI.Shared.Pot.Enums;
using NXAI.Shared.Pot.Json;
using NXAI.Shared.Pot.Mqtt;

namespace NXAI.Shared.Pot.Tests;

/// <summary>第 3 步验收：信封能序列化，且与协议文档字面量对齐。</summary>
public sealed class PotMqttEnvelopeTests
{
    [Fact]
    public void CommandEnvelope_RoundTrips_WithProtocolLiterals()
    {
        var envelope = new PotMqttEnvelope<PotCommandPayload>
        {
            Schema = PotMqttSchemas.CmdV1,
            MsgId = 1937000000001,
            Ts = 1760000000,
            Sn = "POT20260001",
            Type = PotMessageType.COMMAND,
            Payload = new PotCommandPayload
            {
                CommandId = 1937000000002,
                TaskId = 1937000000003,
                StepId = 1937000000004,
                Epoch = 7,
                StepNo = 2,
                Seq = 1,
                IdempotencyKey = "1937000000003:1937000000004:1",
                Action = CommandAction.DISPENSE,
                SlotCode = "S3",
                Mode = DispenseMode.PACKAGE,
                Amount = new PotAmount { Value = 1, Unit = AmountUnit.PACK },
                Trigger = new PotTrigger { Type = TriggerType.TEMP_GTE, TempCelsius = 80 },
                ExpireAt = 1760003600
            }
        };

        var json = PotJson.Serialize(envelope);
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        Assert.Equal("pot.cmd.v1", root.GetProperty("schema").GetString());
        Assert.Equal("COMMAND", root.GetProperty("type").GetString());
        Assert.Equal("POT20260001", root.GetProperty("sn").GetString());

        var payload = root.GetProperty("payload");
        Assert.Equal("DISPENSE", payload.GetProperty("action").GetString());
        Assert.Equal("PACKAGE", payload.GetProperty("mode").GetString());
        Assert.Equal("PACK", payload.GetProperty("amount").GetProperty("unit").GetString());
        Assert.Equal(1, payload.GetProperty("amount").GetProperty("value").GetDecimal());
        Assert.Equal("TEMP_GTE", payload.GetProperty("trigger").GetProperty("type").GetString());
        Assert.Equal(80, payload.GetProperty("trigger").GetProperty("tempCelsius").GetDecimal());

        var restored = PotJson.Deserialize<PotMqttEnvelope<PotCommandPayload>>(json);
        Assert.NotNull(restored);
        Assert.Equal(envelope.MsgId, restored!.MsgId);
        Assert.Equal(envelope.Type, restored.Type);
        Assert.NotNull(restored.Payload);
        Assert.Equal(CommandAction.DISPENSE, restored.Payload!.Action);
        Assert.Equal(DispenseMode.PACKAGE, restored.Payload.Mode);
        Assert.Equal(TriggerType.TEMP_GTE, restored.Payload.Trigger!.Type);
        Assert.Equal(80, restored.Payload.Trigger.TempCelsius);
        Assert.Equal(1, restored.Payload.Amount!.Value);
        Assert.Equal(AmountUnit.PACK, restored.Payload.Amount.Unit);
    }

    [Fact]
    public void AckEnvelope_RoundTrips_ResultAsString()
    {
        var envelope = new PotMqttEnvelope<PotAckPayload>
        {
            Schema = PotMqttSchemas.CmdV1,
            MsgId = 1937000000010,
            Ts = 1760000010,
            Sn = "POT20260001",
            Type = PotMessageType.ACK,
            Payload = new PotAckPayload
            {
                CommandId = 1937000000002,
                TaskId = 1937000000003,
                Epoch = 7,
                StepNo = 2,
                Result = CommandResultCode.RECEIVED,
                WaterTemp = 79.5m
            }
        };

        var json = PotJson.Serialize(envelope);
        Assert.Contains("\"type\":\"ACK\"", json);
        Assert.Contains("\"result\":\"RECEIVED\"", json);

        var restored = PotJson.Deserialize<PotMqttEnvelope<PotAckPayload>>(json);
        Assert.NotNull(restored);
        Assert.Equal(CommandResultCode.RECEIVED, restored!.Payload!.Result);
        Assert.Equal(79.5m, restored.Payload.WaterTemp);
    }
}
