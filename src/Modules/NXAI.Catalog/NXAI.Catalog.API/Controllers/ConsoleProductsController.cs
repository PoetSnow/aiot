using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NXAI.Catalog.Application.Contracts.Dtos;
using NXAI.Catalog.Application.Contracts.Interfaces;
using NXAI.Shared.Application.Contracts.Dtos;
using NXAI.Shared.WebApi.Routing;

namespace NXAI.Catalog.API.Controllers;

/// <summary>货架商品（后台）。</summary>
[Authorize]
[Route($"{ApiSurfaces.ConsoleRoutePrefix}/products")]
public sealed class ConsoleProductsController(ICatalogService catalog) : ConsoleApiController
{
    /// <summary>创建商品。</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<IdDto>> CreateAsync([FromBody] ProductCreationDto input)
        => CreatedResult(await catalog.CreateAsync(input));

    /// <summary>更新商品。</summary>
    [HttpPut("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> UpdateAsync([FromRoute] long id, [FromBody] ProductCreationDto input)
        => Result(await catalog.UpdateAsync(id, input));

    /// <summary>后台商品列表。</summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ProductDto>>> GetListAsync()
        => await catalog.GetConsoleListAsync();
}
