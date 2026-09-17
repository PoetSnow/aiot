using System.Net;
using Microsoft.EntityFrameworkCore;
using NXAI.Catalog.Application.Contracts.Dtos;
using NXAI.Catalog.Application.Contracts.Interfaces;
using NXAI.Infra.IdGenerater.Yitter;
using NXAI.Infra.Repository;
using NXAI.Shared.Application.Contracts.Dtos;
using NXAI.Shared.Application.Contracts.ResultModels;
using ProductEntity = NXAI.Catalog.Repository.Entities.Product;
using ProductStatus = NXAI.Catalog.Repository.Entities.ProductStatus;

namespace NXAI.Catalog.Application.Services;

/// <summary>货架。实现见 <see cref="ICatalogService"/>。</summary>
public sealed class CatalogService(IEfRepository<ProductEntity> products) : ICatalogService
{
    public async Task<ServiceResult<IdDto>> CreateAsync(ProductCreationDto input)
    {
        var sku = input.SkuCode?.Trim() ?? string.Empty;
        if (sku.Length == 0)
        {
            return new ProblemDetails(HttpStatusCode.BadRequest, "SKU 不能为空");
        }

        if (await products.AnyAsync(x => x.SkuCode == sku))
        {
            return new ProblemDetails(HttpStatusCode.Conflict, "SKU 已存在");
        }

        var entity = MapInput(new ProductEntity { Id = IdGenerater.GetNextId(), SkuCode = sku }, input);
        await products.InsertAsync(entity);
        return new IdDto(entity.Id);
    }

    public async Task<ServiceResult> UpdateAsync(long id, ProductCreationDto input)
    {
        var entity = await products.FetchAsync(x => x.Id == id, noTracking: false);
        if (entity is null)
        {
            return new ProblemDetails(HttpStatusCode.NotFound, "商品不存在");
        }

        MapInput(entity, input);
        await products.UpdateAsync(entity);
        return new ServiceResult();
    }

    public async Task<List<ProductDto>> GetConsoleListAsync()
    {
        var list = await products.GetAll().OrderBy(x => x.SkuCode).ToListAsync();
        return list.Select(Map).ToList();
    }

    public async Task<List<ProductDto>> GetPublishedListAsync()
    {
        var list = await products.GetAll().Where(x => x.Status == ProductStatus.OnShelf).OrderBy(x => x.SkuCode).ToListAsync();
        return list.Select(Map).ToList();
    }

    private static ProductEntity MapInput(ProductEntity entity, ProductCreationDto input)
    {
        entity.Name = string.IsNullOrWhiteSpace(input.Name) ? entity.SkuCode : input.Name.Trim();
        entity.Form = string.IsNullOrWhiteSpace(input.Form) ? "PACKAGE" : input.Form.Trim();
        entity.ConsumableTypeCode = string.IsNullOrWhiteSpace(input.ConsumableTypeCode) ? entity.SkuCode : input.ConsumableTypeCode.Trim();
        entity.SuggestedRecipeCode = string.IsNullOrWhiteSpace(input.SuggestedRecipeCode) ? null : input.SuggestedRecipeCode.Trim();
        entity.CompatibleModels = input.CompatibleModels?.Trim() ?? string.Empty;
        entity.PackageQty = input.PackageQty <= 0 ? 1 : input.PackageQty;
        entity.QtyUnit = string.IsNullOrWhiteSpace(input.QtyUnit) ? "PACK" : input.QtyUnit.Trim();
        entity.Status = input.Status == ProductStatus.OffShelf ? ProductStatus.OffShelf : ProductStatus.OnShelf;
        entity.DetailJson = string.IsNullOrWhiteSpace(input.DetailJson) ? "{}" : input.DetailJson.Trim();
        return entity;
    }

    private static ProductDto Map(ProductEntity x) => new()
    {
        Id = x.Id,
        SkuCode = x.SkuCode,
        Name = x.Name,
        Form = x.Form,
        ConsumableTypeCode = x.ConsumableTypeCode,
        SuggestedRecipeCode = x.SuggestedRecipeCode,
        CompatibleModels = x.CompatibleModels,
        PackageQty = x.PackageQty,
        QtyUnit = x.QtyUnit,
        Status = x.Status,
        DetailJson = x.DetailJson
    };
}
