using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NXAI.Cooking.Application.Contracts.Dtos;
using NXAI.Cooking.Application.Contracts.Interfaces;
using NXAI.Shared;
using NXAI.Shared.WebApi.Routing;

namespace NXAI.Cooking.API.Controllers;

/// <summary>制作任务（小程序）。</summary>
[Authorize]
[Route($"{ApiSurfaces.PortalRoutePrefix}/cooking-tasks")]
public sealed class PortalCookingTasksController(ICookingService cooking, UserContext userContext) : PortalApiController
{
    /// <summary>手选配方启动。校验失败零投放。</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<CookingTaskDto>> CreateAsync([FromBody] CookingTaskCreationDto input)
        => CreatedResult(await cooking.CreateAsync(userContext.Id, input));

    /// <summary>任务详情。</summary>
    [HttpGet("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CookingTaskDto>> GetAsync([FromRoute] long id)
    {
        var item = await cooking.GetAsync(userContext.Id, id);
        return item is null ? NotFound() : item;
    }

    /// <summary>取消并下发 STOP。</summary>
    [HttpPost("{id:long}/cancel")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> CancelAsync([FromRoute] long id)
        => Result(await cooking.CancelAsync(userContext.Id, id));
}
