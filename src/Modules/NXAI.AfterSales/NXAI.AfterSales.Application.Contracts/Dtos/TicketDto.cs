using NXAI.Shared.Application.Contracts.Dtos;

namespace NXAI.AfterSales.Application.Contracts.Dtos;

/// <summary>售后工单。</summary>
public class TicketDto : OutputDto
{
    /// <summary>工单号。</summary>
    public string TicketNo { get; set; } = string.Empty;

    /// <summary>SN。</summary>
    public string Sn { get; set; } = string.Empty;

    /// <summary>会员 Id。</summary>
    public long? MemberId { get; set; }

    /// <summary>类型。</summary>
    public int Type { get; set; }

    /// <summary>状态。</summary>
    public int Status { get; set; }

    /// <summary>开单时质保是否有效。</summary>
    public bool WarrantyValid { get; set; }

    /// <summary>症状。</summary>
    public string Symptom { get; set; } = string.Empty;

    /// <summary>结案结果。</summary>
    public string? CloseResult { get; set; }
}
