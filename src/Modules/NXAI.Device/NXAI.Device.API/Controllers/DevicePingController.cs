using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NXAI.Device.API.Controllers;

/// <summary>设备模块探活。</summary>
public sealed class DevicePingController : DeviceApiController
{
    /// <summary>探活。</summary>
    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Get() => Ok(new { module = "Device", surface = "device" });
}
