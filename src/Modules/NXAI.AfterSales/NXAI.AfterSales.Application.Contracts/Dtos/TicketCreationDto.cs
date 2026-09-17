using NXAI.Shared.Application.Contracts.Dtos;

namespace NXAI.AfterSales.Application.Contracts.Dtos;

/// <summary>开售后工单。</summary>
public class TicketCreationDto : InputDto
{
    /// <summary>设备 SN。</summary>
    public string Sn { get; set; } = string.Empty;

    /// <summary>类型：0 咨询 / 1 维修 / 2 退货 / 3 换货。</summary>
    public int Type { get; set; } = 1;

    /// <summary>症状。</summary>
    public string Symptom { get; set; } = string.Empty;
}
