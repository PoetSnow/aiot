using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NXAI.AfterSales.Application.Contracts.Dtos;
using NXAI.AfterSales.Application.Contracts.Interfaces;
using NXAI.Shared;
using NXAI.Shared.Application.Contracts.Dtos;
using NXAI.Shared.WebApi.Routing;

namespace NXAI.AfterSales.API.Controllers;

/// <summary>售后工单（后台）。</summary>
[Authorize]
[Route($"{ApiSurfaces.ConsoleRoutePrefix}/tickets")]
public sealed class ConsoleTicketsController(ITicketService tickets, UserContext userContext) : ConsoleApiController
{
    /// <summary>开单。维修会把 SN 标为维修中并解绑。</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<IdDto>> CreateAsync([FromBody] TicketCreationDto input)
        => CreatedResult(await tickets.CreateByStaffAsync(userContext.Id, input));

    /// <summary>工单列表。</summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<TicketDto>>> GetListAsync()
        => await tickets.GetConsoleListAsync();

    /// <summary>受理。</summary>
    [HttpPost("{id:long}/accept")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> AcceptAsync([FromRoute] long id)
        => Result(await tickets.AcceptAsync(userContext.Id, id));

    /// <summary>结案。</summary>
    [HttpPost("{id:long}/close")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> CloseAsync([FromRoute] long id, [FromBody] TicketCloseDto input)
        => Result(await tickets.CloseAsync(userContext.Id, id, input));
}
