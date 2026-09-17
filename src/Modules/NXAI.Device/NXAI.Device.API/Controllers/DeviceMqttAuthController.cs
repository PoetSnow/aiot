using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NXAI.Device.Application.Contracts.Dtos;
using NXAI.Device.Application.Contracts.Interfaces;
using NXAI.Shared.WebApi.Routing;

namespace NXAI.Device.API.Controllers;

/// <summary>EMQX HTTP 认证。不走设备 JSON 包络，200 放行 / 401 拒绝。</summary>
[AllowAnonymous]
[ApiController]
[ApiExplorerSettings(GroupName = ApiSurfaces.DeviceGroup)]
[Route($"{ApiSurfaces.DeviceRoutePrefix}/mqtt-auth")]
public sealed class DeviceMqttAuthController(IDeviceService devices) : ControllerBase
{
    /// <summary>username=sn，password=deviceToken，clientId=pot-{sn}。</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AuthAsync([FromBody] DeviceMqttAuthDto input)
        => await devices.MqttAuthAsync(input) ? Ok() : Unauthorized();
}
