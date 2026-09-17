using NXAI.Shared.Application.Contracts.Dtos;

namespace NXAI.Asset.Application.Contracts.Dtos;

/// <summary>创建出库单。</summary>
public class StockOutCreationDto : InputDto
{
    /// <summary>出库仓库 Id。</summary>
    public long WarehouseId { get; set; }

    /// <summary>挂接会员 Id，可空。不是员工 Id。</summary>
    public long? MemberId { get; set; }

    /// <summary>备注。</summary>
    public string Remark { get; set; } = string.Empty;

    /// <summary>出库 SN 列表。</summary>
    public List<string> Sns { get; set; } = [];
}
