using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NXAI.Recipe.Application.Contracts.Dtos;
using NXAI.Recipe.Application.Contracts.Interfaces;
using NXAI.Shared.Application.Contracts.Dtos;
using NXAI.Shared.WebApi.Routing;

namespace NXAI.Recipe.API.Controllers;

/// <summary>物料（后台）。</summary>
[Authorize]
[Route($"{ApiSurfaces.ConsoleRoutePrefix}/materials")]
public sealed class MaterialsController(IRecipeService recipes) : ConsoleApiController
{
    /// <summary>创建物料。</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<IdDto>> CreateAsync([FromBody] MaterialCreationDto input)
        => CreatedResult(await recipes.CreateMaterialAsync(input));

    /// <summary>更新物料。</summary>
    [HttpPut("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> UpdateAsync([FromRoute] long id, [FromBody] MaterialCreationDto input)
        => Result(await recipes.UpdateMaterialAsync(id, input));

    /// <summary>物料列表。</summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<MaterialDto>>> GetListAsync()
        => await recipes.GetMaterialsAsync();
}
