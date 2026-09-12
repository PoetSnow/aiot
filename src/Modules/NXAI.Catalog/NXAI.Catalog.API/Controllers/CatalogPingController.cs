using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NXAI.Catalog.API.Controllers;

public sealed class CatalogPingController : ConsoleApiController
{
    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Get() => Ok(new { module = "Catalog", surface = "console" });
}
