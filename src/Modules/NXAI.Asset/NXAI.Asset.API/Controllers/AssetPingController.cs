using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NXAI.Asset.API.Controllers;

/// <summary>资产模块探活。</summary>
public sealed class AssetPingController : ConsoleApiController
{
    /// <summary>探活。</summary>
    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Get() => Ok(new { module = "Asset", surface = "console" });
}
