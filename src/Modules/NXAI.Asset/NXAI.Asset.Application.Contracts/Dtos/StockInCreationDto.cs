using NXAI.Shared.Application.Contracts.Dtos;

namespace NXAI.Asset.Application.Contracts.Dtos;

/// <summary>创建入库单。</summary>
public class StockInCreationDto : InputDto
{
    /// <summary>入库仓库 Id。</summary>
    public long WarehouseId { get; set; }

    /// <summary>备注。</summary>
    public string Remark { get; set; } = string.Empty;

    /// <summary>入库明细，一行一个 SN。</summary>
    public List<StockInLineDto> Lines { get; set; } = [];
}

/// <summary>入库单行。</summary>
public class StockInLineDto : InputDto
{
    /// <summary>SN。</summary>
    public string Sn { get; set; } = string.Empty;

    /// <summary>型号编码。</summary>
    public string ModelCode { get; set; } = string.Empty;
}
