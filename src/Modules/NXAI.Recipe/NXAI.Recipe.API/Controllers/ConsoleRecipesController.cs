using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NXAI.Recipe.Application.Contracts.Dtos;
using NXAI.Recipe.Application.Contracts.Interfaces;
using NXAI.Shared.Application.Contracts.Dtos;
using NXAI.Shared.WebApi.Routing;

namespace NXAI.Recipe.API.Controllers;

/// <summary>配方（后台）。方案 A：草稿可改，发布后旧版只读。</summary>
[Authorize]
[Route($"{ApiSurfaces.ConsoleRoutePrefix}/recipes")]
public sealed class ConsoleRecipesController(IRecipeService recipes) : ConsoleApiController
{
    /// <summary>创建草稿。步骤只写 TargetCode，禁止仓位号。</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<IdDto>> CreateAsync([FromBody] RecipeCreationDto input)
        => CreatedResult(await recipes.CreateRecipeAsync(input));

    /// <summary>只改草稿。</summary>
    [HttpPut("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> UpdateAsync([FromRoute] long id, [FromBody] RecipeCreationDto input)
        => Result(await recipes.UpdateRecipeAsync(id, input));

    /// <summary>后台配方列表。</summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<RecipeDto>>> GetListAsync()
        => await recipes.GetConsoleListAsync();

    /// <summary>某一版详情。</summary>
    [HttpGet("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RecipeDto>> GetAsync([FromRoute] long id)
    {
        var item = await recipes.GetByIdAsync(id);
        return item is null ? NotFound() : item;
    }

    /// <summary>发布。同一 Code 只保留一条 Published。</summary>
    [HttpPost("{id:long}/publish")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> PublishAsync([FromRoute] long id)
        => Result(await recipes.PublishAsync(id));
}
