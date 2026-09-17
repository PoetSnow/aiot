using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NXAI.Asset.Application.Contracts.Dtos;
using NXAI.Asset.Application.Contracts.Interfaces;
using NXAI.Shared.Application.Contracts.Dtos;
using NXAI.Shared.WebApi.Routing;

namespace NXAI.Asset.API.Controllers;

/// <summary>仓库（后台）。</summary>
[Authorize]
[Route($"{ApiSurfaces.ConsoleRoutePrefix}/warehouses")]
public sealed class WarehousesController(IWarehouseService warehouses) : ConsoleApiController
{
    /// <summary>创建仓库。启动会种子 MAIN，也可手工加仓。</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<IdDto>> CreateAsync([FromBody] WarehouseCreationDto input)
        => CreatedResult(await warehouses.CreateAsync(input));

    /// <summary>仓库列表。</summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<WarehouseDto>>> GetListAsync()
        => await warehouses.GetListAsync();
}
