using Microsoft.AspNetCore.Mvc;
using NXAI.Device.Application.Contracts.Dtos;
using NXAI.Device.Application.Contracts.Interfaces;
using NXAI.Shared.WebApi.Routing;

namespace NXAI.Device.API.Controllers;

/// <summary>开发用：EMQX 未起时把设备标在线，便于测 Cooking 校验。</summary>
[Route(ApiSurfaces.InternalRoutePrefix)]
public sealed class DevDeviceController(IDeviceService devices) : InternalApiController
{
    /// <summary>按 SN 设置在线。模拟器连上后会覆盖。</summary>
    [HttpPost("dev/devices/{sn}/online")]
    public async Task<IActionResult> SetOnlineAsync([FromRoute] string sn, [FromQuery] bool online = true)
        => Result(await devices.SetOnlineBySnAsync(sn, online));

    /// <summary>写入影子，不当投放扳机。</summary>
    [HttpPost("dev/devices/{sn}/shadow")]
    public async Task<IActionResult> ShadowAsync([FromRoute] string sn, [FromBody] DeviceShadowReportDto input)
    {
        await devices.ApplyShadowAsync(sn, input);
        return NoContent();
    }
}
