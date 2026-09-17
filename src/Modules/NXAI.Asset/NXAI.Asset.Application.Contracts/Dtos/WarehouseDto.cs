using NXAI.Shared.Application.Contracts.Dtos;

namespace NXAI.Asset.Application.Contracts.Dtos;

/// <summary>仓库。</summary>
/// <param name="Id">主键。</param>
/// <param name="Code">编码。</param>
/// <param name="Name">名称。</param>
public record WarehouseDto(long Id, string Code, string Name) : IDto;
