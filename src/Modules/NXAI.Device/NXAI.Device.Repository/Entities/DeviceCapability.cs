using NXAI.Infra.Repository;

namespace NXAI.Device.Repository.Entities;

/// <summary>设备能力快照，表 dev_capability。上线必报，Cooking 按此校验。</summary>
public class DeviceCapability : EfEntity
{
    /// <summary>能力 JSON 最大长度。</summary>
    public const int PayloadJson_MaxLength = 4000;

    /// <summary>所属设备，一对一。</summary>
    public long DeviceId { get; set; }

    /// <summary>仓、模式、actions 的 JSON。</summary>
    public string PayloadJson { get; set; } = "{}";

    /// <summary>最近上报时间。</summary>
    public DateTime? ReportedAt { get; set; }
}
