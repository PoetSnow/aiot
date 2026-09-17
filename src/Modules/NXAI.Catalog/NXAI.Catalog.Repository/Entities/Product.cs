using NXAI.Infra.Repository;

namespace NXAI.Catalog.Repository.Entities;

/// <summary>耗材货架 SKU，表 cat_product。不当投料单元，也不当 SN 库存。</summary>
public class Product : EfEntity
{
    public const int SkuCode_MaxLength = 32;
    public const int Name_MaxLength = 64;
    public const int Form_MaxLength = 16;
    public const int ConsumableTypeCode_MaxLength = 32;
    public const int SuggestedRecipeCode_MaxLength = 32;
    public const int CompatibleModels_MaxLength = 256;
    public const int QtyUnit_MaxLength = 8;
    public const int DetailJson_MaxLength = 4000;

    /// <summary>SKU 编码。</summary>
    public string SkuCode { get; set; } = string.Empty;

    /// <summary>名称。</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>形态，如 PACKAGE。</summary>
    public string Form { get; set; } = "PACKAGE";

    /// <summary>耗材类型，与配方 TargetCode 对齐。</summary>
    public string ConsumableTypeCode { get; set; } = string.Empty;

    /// <summary>建议配方编码。</summary>
    public string? SuggestedRecipeCode { get; set; }

    /// <summary>适配型号，逗号分隔。</summary>
    public string CompatibleModels { get; set; } = string.Empty;

    /// <summary>每份数量。</summary>
    public decimal PackageQty { get; set; } = 1;

    /// <summary>数量单位。</summary>
    public string QtyUnit { get; set; } = "PACK";

    /// <summary>状态，见 <see cref="ProductStatus"/>。</summary>
    public int Status { get; set; } = ProductStatus.OnShelf;

    /// <summary>详情 JSON。禁止医疗宣称。</summary>
    public string DetailJson { get; set; } = "{}";
}

/// <summary>货架状态。</summary>
public static class ProductStatus
{
    /// <summary>下架，小程序不可见。</summary>
    public const int OffShelf = 0;

    /// <summary>上架。</summary>
    public const int OnShelf = 1;
}
