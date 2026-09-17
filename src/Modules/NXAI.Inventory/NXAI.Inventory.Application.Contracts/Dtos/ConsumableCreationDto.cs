using NXAI.Shared.Application.Contracts.Dtos;

namespace NXAI.Inventory.Application.Contracts.Dtos;

/// <summary>创建耗材实例。</summary>
public class ConsumableCreationDto : InputDto
{
    /// <summary>类型编码，与配方 TargetCode 对齐，如 TEA_PACK。</summary>
    public string ConsumableTypeCode { get; set; } = string.Empty;

    /// <summary>货架商品 Id，可空。</summary>
    public long? ProductId { get; set; }

    /// <summary>形态。</summary>
    public string Form { get; set; } = "PACKAGE";

    /// <summary>数量，整包不可拆。</summary>
    public decimal Qty { get; set; } = 1;

    /// <summary>单位。</summary>
    public string QtyUnit { get; set; } = "PACK";
}
