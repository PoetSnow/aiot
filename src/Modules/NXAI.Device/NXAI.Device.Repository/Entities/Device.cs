using NXAI.Infra.Repository;

namespace NXAI.Device.Repository.Entities;

/// <summary>已绑定设备，表 dev_device。MemberId 不是员工 Id。</summary>
public class Device : EfEntity
{
    /// <summary>SN 最大长度。</summary>
    public const int DeviceSn_MaxLength = 64;

    /// <summary>型号编码最大长度。</summary>
    public const int ModelCode_MaxLength = 32;

    /// <summary>设备 Token 哈希最大长度。</summary>
    public const int TokenHash_MaxLength = 64;

    /// <summary>主人会员 Id。</summary>
    public long MemberId { get; set; }

    /// <summary>设备 SN，全局唯一。</summary>
    public string DeviceSn { get; set; } = string.Empty;

    /// <summary>型号编码，来自 Asset。</summary>
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>当前在途任务。一壶同时一个 ActiveTask。</summary>
    public long? ActiveTaskId { get; set; }

    /// <summary>当前任务世代。旧 epoch 包一律作废。</summary>
    public int ActiveEpoch { get; set; }

    /// <summary>设备访问令牌 SHA256 十六进制。明文只下发一次。</summary>
    public string DeviceTokenHash { get; set; } = string.Empty;

    /// <summary>最近激活时间。</summary>
    public DateTime? ActivatedAt { get; set; }
}

/// <summary>仓位绑定种类。</summary>
public static class SlotBindingKind
{
    /// <summary>空仓。</summary>
    public const int None = 0;

    /// <summary>按物料编码占位，尚未落到耗材实例。</summary>
    public const int Material = 1;

    /// <summary>绑定了会员耗材实例。第 8 步 Inventory 才有实例。</summary>
    public const int Consumable = 2;
}

/// <summary>MQTT Outbox 投递状态。</summary>
public static class MqttOutboxStatus
{
    /// <summary>未收到 RECEIVED，可重发同一 commandId。</summary>
    public const int Pending = 0;

    /// <summary>已 RECEIVED，禁止当新投放重发。</summary>
    public const int Received = 1;
}
