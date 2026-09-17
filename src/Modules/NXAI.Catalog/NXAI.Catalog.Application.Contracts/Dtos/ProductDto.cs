using NXAI.Shared.Application.Contracts.Dtos;

namespace NXAI.Catalog.Application.Contracts.Dtos;

/// <summary>货架商品。</summary>
public class ProductDto : OutputDto
{
    /// <summary>SKU 编码。</summary>
    public string SkuCode { get; set; } = string.Empty;

    /// <summary>名称。</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>形态。</summary>
    public string Form { get; set; } = string.Empty;

    /// <summary>耗材类型。</summary>
    public string ConsumableTypeCode { get; set; } = string.Empty;

    /// <summary>建议配方。</summary>
    public string? SuggestedRecipeCode { get; set; }

    /// <summary>适配型号。</summary>
    public string CompatibleModels { get; set; } = string.Empty;

    /// <summary>每份数量。</summary>
    public decimal PackageQty { get; set; }

    /// <summary>数量单位。</summary>
    public string QtyUnit { get; set; } = string.Empty;

    /// <summary>1 上架 / 0 下架。</summary>
    public int Status { get; set; }

    /// <summary>详情 JSON。</summary>
    public string DetailJson { get; set; } = "{}";
}
