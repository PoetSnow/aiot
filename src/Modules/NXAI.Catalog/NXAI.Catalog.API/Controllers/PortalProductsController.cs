using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NXAI.Catalog.Application.Contracts.Dtos;
using NXAI.Catalog.Application.Contracts.Interfaces;
using NXAI.Shared.WebApi.Routing;

namespace NXAI.Catalog.API.Controllers;

/// <summary>货架（小程序只读上架）。</summary>
[Authorize]
[Route($"{ApiSurfaces.PortalRoutePrefix}/products")]
public sealed class PortalProductsController(ICatalogService catalog) : PortalApiController
{
    /// <summary>上架商品。</summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ProductDto>>> GetListAsync()
        => await catalog.GetPublishedListAsync();
}
