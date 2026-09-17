using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NXAI.Device.Application.Contracts.Dtos;
using NXAI.Device.Application.Contracts.Interfaces;
using NXAI.Shared.WebApi.Routing;

namespace NXAI.Device.API.Controllers;

/// <summary>设备激活与换 Token。投放不走 HTTP。</summary>
[AllowAnonymous]
[Route($"{ApiSurfaces.DeviceRoutePrefix}")]
public sealed class DeviceAccessController(IDeviceService devices) : DeviceApiController
{
    /// <summary>请求头：设备访问令牌。</summary>
    public const string DeviceTokenHeader = "X-Device-Token";

    /// <summary>已绑定 SN 激活，下发设备 Token。</summary>
    [HttpPost("activate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<DeviceTokenDto>> ActivateAsync([FromBody] DeviceActivateDto input)
        => Result(await devices.ActivateAsync(input));

    /// <summary>用当前 Token 换新 Token。</summary>
    [HttpPost("token/refresh")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<DeviceTokenDto>> RefreshTokenAsync()
    {
        var token = Request.Headers[DeviceTokenHeader].ToString();
        if (string.IsNullOrWhiteSpace(token))
        {
            return Unauthorized();
        }

        return Result(await devices.RefreshTokenAsync(token));
    }

    /// <summary>设备对时，Unix 秒。</summary>
    [HttpGet("time")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetTime() => Ok(new { unix = DateTimeOffset.UtcNow.ToUnixTimeSeconds() });
}
