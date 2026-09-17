using NXAI.Catalog.Application.Contracts.Dtos;
using NXAI.Shared.Application.Contracts.Dtos;
using NXAI.Shared.Application.Contracts.ResultModels;

namespace NXAI.Catalog.Application.Contracts.Interfaces;

/// <summary>耗材货架。后台维护，小程序只读上架。</summary>
public interface ICatalogService
{
    /// <summary>创建商品。</summary>
    Task<ServiceResult<IdDto>> CreateAsync(ProductCreationDto input);

    /// <summary>更新商品。</summary>
    Task<ServiceResult> UpdateAsync(long id, ProductCreationDto input);

    /// <summary>后台全部商品。</summary>
    Task<List<ProductDto>> GetConsoleListAsync();

    /// <summary>小程序只读上架商品。</summary>
    Task<List<ProductDto>> GetPublishedListAsync();
}
