using NXAI.Infra.Repository;

namespace NXAI.Device.Repository.Entities;

/// <summary>MQTT 下行 Outbox，表 dev_mqtt_outbox。重试同一 commandId，直到 RECEIVED。</summary>
public class DeviceMqttOutbox : EfEntity
{
    /// <summary>载荷 JSON 最大长度。</summary>
    public const int PayloadJson_MaxLength = 8000;

    /// <summary>所属设备。</summary>
    public long DeviceId { get; set; }

    /// <summary>指令 Id。已 RECEIVED 禁止当新投放重发。</summary>
    public long CommandId { get; set; }

    /// <summary>COMMAND 信封 JSON。</summary>
    public string PayloadJson { get; set; } = "{}";

    /// <summary>投递状态，见 <see cref="MqttOutboxStatus"/>。</summary>
    public int Status { get; set; } = MqttOutboxStatus.Pending;

    /// <summary>重试次数。</summary>
    public int RetryCount { get; set; }

    /// <summary>最近一次投递时间。第 6 步才真正发 MQTT。</summary>
    public DateTime? LastAttemptAt { get; set; }
}
