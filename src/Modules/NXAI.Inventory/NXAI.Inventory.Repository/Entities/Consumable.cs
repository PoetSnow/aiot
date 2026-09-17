using NXAI.Infra.Repository;

namespace NXAI.Inventory.Repository.Entities;

/// <summary>会员耗材实例，表 inv_consumable。放置位置只是投影，仓位事实在 Device。</summary>
public class Consumable : EfEntity
{
    public const int ConsumableTypeCode_MaxLength = 32;
    public const int Form_MaxLength = 16;
    public const int QtyUnit_MaxLength = 8;
    public const int SlotCode_MaxLength = 16;

    /// <summary>主人会员 Id。</summary>
    public long MemberId { get; set; }

    /// <summary>类型编码，与配方 TargetCode 对齐。</summary>
    public string ConsumableTypeCode { get; set; } = string.Empty;

    /// <summary>货架商品 Id，可空。</summary>
    public long? ProductId { get; set; }

    /// <summary>形态。</summary>
    public string Form { get; set; } = "PACKAGE";

    /// <summary>初始数量。</summary>
    public decimal TotalQty { get; set; }

    /// <summary>剩余数量。</summary>
    public decimal RemainQty { get; set; }

    /// <summary>数量单位。</summary>
    public string QtyUnit { get; set; } = "PACK";

    /// <summary>状态，见 <see cref="ConsumableStatus"/>。</summary>
    public int Status { get; set; } = ConsumableStatus.Available;

    /// <summary>放置设备投影。</summary>
    public long? PlacedDeviceId { get; set; }

    /// <summary>放置仓位投影。</summary>
    public string? PlacedSlotCode { get; set; }
}

/// <summary>耗材状态。</summary>
public static class ConsumableStatus
{
    /// <summary>可用。</summary>
    public const int Available = 0;

    /// <summary>已用尽。</summary>
    public const int Empty = 1;
}
