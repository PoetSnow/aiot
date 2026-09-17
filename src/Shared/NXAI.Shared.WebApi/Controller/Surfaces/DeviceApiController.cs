using Microsoft.AspNetCore.Authorization;
using NXAI.Shared.WebApi.Filters;
using NXAI.Shared.WebApi.Routing;

namespace Microsoft.AspNetCore.Mvc;

/// <summary>设备端 API 基类，路由 <c>api/device/...</c>。鉴权用 <c>X-Device-Token</c>，不走 JWT。</summary>
[ApiController]
[ApiExplorerSettings(GroupName = ApiSurfaces.DeviceGroup)]
[Route(ApiSurfaces.DeviceRouteTemplate)]
[AllowAnonymous]
[ServiceFilter(typeof(DeviceApiEnvelopeResultFilter))]
public abstract class DeviceApiController : NXAIControllerBase;
