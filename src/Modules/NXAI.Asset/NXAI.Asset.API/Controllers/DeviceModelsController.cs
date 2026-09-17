using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NXAI.Asset.Application.Contracts.Dtos;
using NXAI.Asset.Application.Contracts.Interfaces;
using NXAI.Shared.Application.Contracts.Dtos;
using NXAI.Shared.WebApi.Routing;

namespace NXAI.Asset.API.Controllers;

/// <summary>设备型号（后台）。</summary>
[Authorize]
[Route($"{ApiSurfaces.ConsoleRoutePrefix}/device-models")]
public sealed class DeviceModelsController(IDeviceModelService deviceModels) : ConsoleApiController
{
    /// <summary>创建型号。</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<IdDto>> CreateAsync([FromBody] DeviceModelCreationDto input)
        => CreatedResult(await deviceModels.CreateAsync(input));

    /// <summary>型号列表。</summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<DeviceModelDto>>> GetListAsync()
        => await deviceModels.GetListAsync();
}
