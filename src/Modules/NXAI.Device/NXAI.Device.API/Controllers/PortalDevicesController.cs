using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NXAI.Device.Application.Contracts.Dtos;
using NXAI.Device.Application.Contracts.Interfaces;
using NXAI.Shared;
using NXAI.Shared.Application.Contracts.Dtos;
using NXAI.Shared.WebApi.Routing;

namespace NXAI.Device.API.Controllers;

/// <summary>小程序设备绑定与仓位。</summary>
[Authorize]
[Route($"{ApiSurfaces.PortalRoutePrefix}/devices")]
public sealed class PortalDevicesController(IDeviceService devices, UserContext userContext) : PortalApiController
{
    /// <summary>绑定已出库 SN。未出库返回冲突。</summary>
    [HttpPost("bind")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<IdDto>> BindAsync([FromBody] DeviceBindDto input)
        => CreatedResult(await devices.BindAsync(userContext.Id, input));

    /// <summary>当前会员的设备。</summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<DeviceDto>>> GetListAsync()
        => await devices.GetByMemberAsync(userContext.Id);

    /// <summary>设备详情。</summary>
    [HttpGet("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DeviceDto>> GetAsync([FromRoute] long id)
    {
        var item = await devices.GetAsync(id, userContext.Id);
        return item is null ? NotFound() : item;
    }

    /// <summary>设备影子。只展示，不用于开火。</summary>
    [HttpGet("{id:long}/shadow")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DeviceShadowDto>> GetShadowAsync([FromRoute] long id)
    {
        var item = await devices.GetShadowAsync(id, userContext.Id);
        return item is null ? NotFound() : item;
    }

    /// <summary>仓位列表。</summary>
    [HttpGet("{id:long}/slots")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<DeviceSlotDto>>> GetSlotsAsync([FromRoute] long id)
        => await devices.GetSlotsAsync(id, userContext.Id);

    /// <summary>绑定仓位物料或耗材。</summary>
    [HttpPut("{id:long}/slots/{slotCode}/binding")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> BindSlotAsync(
        [FromRoute] long id,
        [FromRoute] string slotCode,
        [FromBody] DeviceSlotBindingDto input)
        => Result(await devices.BindSlotAsync(id, slotCode, userContext.Id, input));
}
