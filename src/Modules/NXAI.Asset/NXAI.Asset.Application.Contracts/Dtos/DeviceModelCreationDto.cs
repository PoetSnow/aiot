using NXAI.Shared.Application.Contracts.Dtos;

namespace NXAI.Asset.Application.Contracts.Dtos;

/// <summary>创建设备型号。</summary>
public class DeviceModelCreationDto : InputDto
{
    /// <summary>型号编码，全局唯一。</summary>
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>型号名称。</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>保修月数。</summary>
    public int WarrantyMonths { get; set; } = 12;

    /// <summary>仓位配置 JSON。默认空数组。</summary>
    public string SlotProfileJson { get; set; } = "[]";
}
