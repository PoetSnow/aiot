using NXAI.Infra.Repository;

namespace NXAI.Asset.Repository.Entities;

/// <summary>设备型号，表 ast_device_model。</summary>
public class DeviceModel : EfEntity
{
    /// <summary>型号编码最大长度。</summary>
    public const int ModelCode_MaxLength = 32;

    /// <summary>型号名称最大长度。</summary>
    public const int Name_MaxLength = 64;

    /// <summary>仓位配置 JSON 最大长度。</summary>
    public const int SlotProfileJson_MaxLength = 4000;

    /// <summary>型号编码，全局唯一。</summary>
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>型号名称。</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>保修月数。</summary>
    public int WarrantyMonths { get; set; }

    /// <summary>仓位配置 JSON，绑定设备时用来建仓。</summary>
    public string SlotProfileJson { get; set; } = "[]";
}
