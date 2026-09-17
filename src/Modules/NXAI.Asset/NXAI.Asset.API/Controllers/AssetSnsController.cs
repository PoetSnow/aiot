using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NXAI.Asset.Application.Contracts.Dtos;
using NXAI.Asset.Application.Contracts.Interfaces;
using NXAI.Shared.WebApi.Routing;

namespace NXAI.Asset.API.Controllers;

/// <summary>SN 台账查询（后台）。</summary>
[Authorize]
[Route($"{ApiSurfaces.ConsoleRoutePrefix}/asset-sns")]
public sealed class AssetSnsController(IAssetSnService assetSns) : ConsoleApiController
{
    /// <summary>按条件查 SN。</summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<AssetSnDto>>> GetListAsync(
        [FromQuery] long? warehouseId,
        [FromQuery] string? modelCode,
        [FromQuery] int? status)
        => await assetSns.GetListAsync(warehouseId, modelCode, status);

    /// <summary>按 SN 查详情。</summary>
    [HttpGet("{sn}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AssetSnDto>> GetBySnAsync([FromRoute] string sn)
    {
        var item = await assetSns.GetBySnAsync(sn);
        return item is null ? NotFound() : item;
    }
}
