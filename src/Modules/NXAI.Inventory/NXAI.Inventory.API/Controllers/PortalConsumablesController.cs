using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NXAI.Inventory.Application.Contracts.Dtos;
using NXAI.Inventory.Application.Contracts.Interfaces;
using NXAI.Shared;
using NXAI.Shared.Application.Contracts.Dtos;
using NXAI.Shared.WebApi.Routing;

namespace NXAI.Inventory.API.Controllers;

/// <summary>会员耗材（小程序）。</summary>
[Authorize]
[Route($"{ApiSurfaces.PortalRoutePrefix}/consumables")]
public sealed class PortalConsumablesController(IInventoryService inventory, UserContext userContext) : PortalApiController
{
    /// <summary>录入耗材。整包不要填内部克数。</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<IdDto>> CreateAsync([FromBody] ConsumableCreationDto input)
        => CreatedResult(await inventory.CreateAsync(userContext.Id, input));

    /// <summary>当前会员耗材。</summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ConsumableDto>>> GetListAsync()
        => await inventory.GetByMemberAsync(userContext.Id);

    /// <summary>放到仓位，例如 S3。</summary>
    [HttpPost("{id:long}/place")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> PlaceAsync([FromRoute] long id, [FromBody] ConsumablePlaceDto input)
        => Result(await inventory.PlaceAsync(userContext.Id, id, input));
}
