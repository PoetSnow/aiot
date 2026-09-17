using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NXAI.Device.Application.Contracts.Dtos;
using NXAI.Device.Application.Contracts.Interfaces;
using NXAI.Shared.WebApi.Routing;

namespace NXAI.Device.API.Controllers;

/// <summary>后台设备与影子查询。</summary>
[Authorize]
[Route($"{ApiSurfaces.ConsoleRoutePrefix}/devices")]
public sealed class ConsoleDevicesController(IDeviceService devices, ITelemetryStore telemetry) : ConsoleApiController
{
    /// <summary>全部已绑定设备。</summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<DeviceDto>>> GetListAsync()
        => await devices.GetConsoleListAsync();

    /// <summary>设备影子。</summary>
    [HttpGet("{id:long}/shadow")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DeviceShadowDto>> GetShadowAsync([FromRoute] long id)
    {
        var item = await devices.GetShadowAsync(id, memberId: null);
        return item is null ? NotFound() : item;
    }

    /// <summary>按时查询遥测。影子是当前值，本接口看这段时间怎么走。</summary>
    [HttpGet("{id:long}/telemetry")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IReadOnlyList<TelemetryPointDto>>> GetTelemetryAsync(
        [FromRoute] long id,
        [FromQuery] string metric = "water_temp",
        [FromQuery] DateTime? from = null,
        [FromQuery] DateTime? to = null,
        [FromQuery] long? taskId = null,
        [FromQuery] int limit = 200)
    {
        var device = await devices.GetAsync(id, memberId: null);
        if (device is null)
        {
            return NotFound();
        }

        var start = from ?? DateTime.UtcNow.AddDays(-1);
        var end = to ?? DateTime.UtcNow.AddMinutes(1);
        return Ok(await telemetry.QueryAsync(device.DeviceSn, metric, start, end, taskId, limit));
    }
}
