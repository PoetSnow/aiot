using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NXAI.Recipe.Application.Contracts.Dtos;
using NXAI.Recipe.Application.Contracts.Interfaces;
using NXAI.Shared.WebApi.Routing;

namespace NXAI.Recipe.API.Controllers;

/// <summary>已发布配方（小程序只读）。</summary>
[Authorize]
[Route($"{ApiSurfaces.PortalRoutePrefix}/recipes")]
public sealed class PortalRecipesController(IRecipeService recipes) : PortalApiController
{
    /// <summary>当前已发布配方。</summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<RecipeDto>>> GetListAsync()
        => await recipes.GetPublishedListAsync();

    /// <summary>按编码取当前 Published 快照。</summary>
    [HttpGet("{code}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RecipeDto>> GetByCodeAsync([FromRoute] string code)
    {
        var item = await recipes.GetPublishedByCodeAsync(code);
        return item is null ? NotFound() : item;
    }
}
