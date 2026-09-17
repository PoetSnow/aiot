using NXAI.Shared.Application.Contracts.Dtos;

namespace NXAI.Inventory.Application.Contracts.Dtos;

/// <summary>耗材实例。</summary>
public class ConsumableDto : OutputDto
{
    /// <summary>类型编码。</summary>
    public string ConsumableTypeCode { get; set; } = string.Empty;

    /// <summary>形态。</summary>
    public string Form { get; set; } = string.Empty;

    /// <summary>剩余数量。</summary>
    public decimal RemainQty { get; set; }

    /// <summary>单位。</summary>
    public string QtyUnit { get; set; } = string.Empty;

    /// <summary>状态：0 可用 / 1 用尽。</summary>
    public int Status { get; set; }

    /// <summary>放置设备。</summary>
    public long? PlacedDeviceId { get; set; }

    /// <summary>放置仓位。</summary>
    public string? PlacedSlotCode { get; set; }
}
