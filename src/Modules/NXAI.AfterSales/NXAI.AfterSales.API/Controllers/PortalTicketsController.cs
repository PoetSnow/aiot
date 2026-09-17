using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NXAI.AfterSales.Application.Contracts.Dtos;
using NXAI.AfterSales.Application.Contracts.Interfaces;
using NXAI.Shared;
using NXAI.Shared.Application.Contracts.Dtos;
using NXAI.Shared.WebApi.Routing;

namespace NXAI.AfterSales.API.Controllers;

/// <summary>售后工单（小程序）。</summary>
[Authorize]
[Route($"{ApiSurfaces.PortalRoutePrefix}/tickets")]
public sealed class PortalTicketsController(ITicketService tickets, UserContext userContext) : PortalApiController
{
    /// <summary>会员开单。</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<IdDto>> CreateAsync([FromBody] TicketCreationDto input)
        => CreatedResult(await tickets.CreateByMemberAsync(userContext.Id, input));

    /// <summary>我的工单。</summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<TicketDto>>> GetListAsync()
        => await tickets.GetByMemberAsync(userContext.Id);
}
