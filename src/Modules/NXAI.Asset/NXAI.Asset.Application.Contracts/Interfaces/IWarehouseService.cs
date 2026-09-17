using NXAI.Asset.Application.Contracts.Dtos;
using NXAI.Shared.Application.Contracts.Dtos;
using NXAI.Shared.Application.Contracts.ResultModels;

namespace NXAI.Asset.Application.Contracts.Interfaces;

/// <summary>仓库维护。</summary>
public interface IWarehouseService
{
    /// <summary>创建仓库。</summary>
    Task<ServiceResult<IdDto>> CreateAsync(WarehouseCreationDto input);

    /// <summary>仓库列表。</summary>
    Task<List<WarehouseDto>> GetListAsync();
}
