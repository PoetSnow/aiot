using NXAI.Shared.Application.Contracts.Dtos;

namespace NXAI.Asset.Application.Contracts.Dtos;

/// <summary>设备型号。</summary>
/// <param name="Id">主键。</param>
/// <param name="ModelCode">型号编码。</param>
/// <param name="Name">名称。</param>
/// <param name="WarrantyMonths">保修月数。</param>
/// <param name="SlotProfileJson">仓位配置 JSON。</param>
public record DeviceModelDto(long Id, string ModelCode, string Name, int WarrantyMonths, string SlotProfileJson) : IDto;
