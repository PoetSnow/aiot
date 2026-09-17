using NXAI.Infra.Repository;

namespace NXAI.Asset.Repository.Entities;

/// <summary>入库单，表 ast_stock_in。未确认禁止改 SN 台账。</summary>
public class StockIn : EfEntity
{
    /// <summary>单号最大长度。</summary>
    public const int BillNo_MaxLength = 32;

    /// <summary>备注最大长度。</summary>
    public const int Remark_MaxLength = 256;

    /// <summary>业务单号。</summary>
    public string BillNo { get; set; } = string.Empty;

    /// <summary>入库仓库。</summary>
    public long WarehouseId { get; set; }

    /// <summary>单据状态，见 <see cref="StockDocStatus"/>。</summary>
    public int Status { get; set; } = StockDocStatus.Draft;

    /// <summary>备注。</summary>
    public string Remark { get; set; } = string.Empty;

    /// <summary>确认时间。</summary>
    public DateTime? ConfirmedAt { get; set; }
}

/// <summary>入出库单据状态。未审禁止改 SN。</summary>
public static class StockDocStatus
{
    /// <summary>草稿，未改 SN 台账。</summary>
    public const int Draft = 0;

    /// <summary>已确认。</summary>
    public const int Confirmed = 1;
}
