using NXAI.Shared.Application.Contracts.Dtos;

namespace NXAI.AfterSales.Application.Contracts.Dtos;

/// <summary>结案。</summary>
public class TicketCloseDto : InputDto
{
    /// <summary>return / scrap / replace。</summary>
    public string Result { get; set; } = "return";
}
