using NXAI.Infra.Repository;

namespace NXAI.Asset.Repository.Entities;

/// <summary>出库单，表 ast_stock_out。</summary>
public class StockOut : EfEntity
{
    /// <summary>单号最大长度。</summary>
    public const int BillNo_MaxLength = 32;

    /// <summary>备注最大长度。</summary>
    public const int Remark_MaxLength = 256;

    /// <summary>业务单号。</summary>
    public string BillNo { get; set; } = string.Empty;

    /// <summary>出库仓库。</summary>
    public long WarehouseId { get; set; }

    /// <summary>单据状态，见 <see cref="StockDocStatus"/>。</summary>
    public int Status { get; set; } = StockDocStatus.Draft;

    /// <summary>出库挂接会员。可空；有值时走 IMemberGateway 校验。</summary>
    public long? MemberId { get; set; }

    /// <summary>备注。</summary>
    public string Remark { get; set; } = string.Empty;

    /// <summary>确认时间。</summary>
    public DateTime? ConfirmedAt { get; set; }
}
