using NXAI.Infra.Repository;

namespace NXAI.Asset.Repository.Entities;

/// <summary>设备 SN 台账，表 ast_sn。库存 = 按仓+型号统计 Status=InStock。</summary>
public class AssetSn : EfEntity
{
    /// <summary>SN 最大长度。</summary>
    public const int Sn_MaxLength = 64;

    /// <summary>型号编码最大长度。</summary>
    public const int ModelCode_MaxLength = 32;

    /// <summary>SN，全局唯一。</summary>
    public string Sn { get; set; } = string.Empty;

    /// <summary>型号编码。</summary>
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>当前所在仓库。出库后仍保留最后仓库便于追溯。</summary>
    public long WarehouseId { get; set; }

    /// <summary>SN 状态，见 <see cref="AssetSnStatus"/>。</summary>
    public int Status { get; set; } = AssetSnStatus.Created;

    /// <summary>出库挂接的会员 Id。不是 sys_user.Id。</summary>
    public long? MemberId { get; set; }

    /// <summary>最近一次入库单 Id。</summary>
    public long? StockInId { get; set; }

    /// <summary>最近一次出库单 Id。</summary>
    public long? StockOutId { get; set; }

    /// <summary>入库确认时间。</summary>
    public DateTime? InboundAt { get; set; }

    /// <summary>出库确认时间。</summary>
    public DateTime? OutboundAt { get; set; }
}

/// <summary>SN 生命周期状态。</summary>
public static class AssetSnStatus
{
    /// <summary>已创建，尚未入库确认。</summary>
    public const int Created = 0;

    /// <summary>在库。未出库小程序不能绑定。</summary>
    public const int InStock = 1;

    /// <summary>已出库，可被会员认领绑定。</summary>
    public const int Outbound = 2;

    /// <summary>已绑定设备会话。</summary>
    public const int Bound = 3;

    /// <summary>维修中。</summary>
    public const int Repairing = 4;

    /// <summary>退货。</summary>
    public const int Returned = 5;

    /// <summary>已换机。</summary>
    public const int Replaced = 6;

    /// <summary>报废。</summary>
    public const int Scrapped = 7;
}
