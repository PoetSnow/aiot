using NXAI.Infra.Repository;

namespace NXAI.Device.Repository.Entities;

/// <summary>设备仓位事实，表 dev_slot。放了什么以本表为准。</summary>
public class DeviceSlot : EfEntity
{
    /// <summary>仓位码最大长度。</summary>
    public const int SlotCode_MaxLength = 16;

    /// <summary>仓位类型最大长度。</summary>
    public const int SlotType_MaxLength = 16;

    /// <summary>支持的投料模式最大长度。</summary>
    public const int SupportedModes_MaxLength = 64;

    /// <summary>物料编码最大长度。</summary>
    public const int MaterialCode_MaxLength = 32;

    /// <summary>所属设备。</summary>
    public long DeviceId { get; set; }

    /// <summary>仓位码，如 S3。</summary>
    public string SlotCode { get; set; } = string.Empty;

    /// <summary>仓位类型：BULK / PACKAGE / CARTRIDGE。</summary>
    public string SlotType { get; set; } = "PACKAGE";

    /// <summary>支持的投料模式，逗号分隔。</summary>
    public string SupportedModes { get; set; } = "PACKAGE";

    /// <summary>绑定种类，见 <see cref="SlotBindingKind"/>。</summary>
    public int BindingKind { get; set; } = SlotBindingKind.None;

    /// <summary>物料编码。</summary>
    public string? MaterialCode { get; set; }

    /// <summary>耗材实例 Id。第 8 步才有值。</summary>
    public long? ConsumableId { get; set; }
}
