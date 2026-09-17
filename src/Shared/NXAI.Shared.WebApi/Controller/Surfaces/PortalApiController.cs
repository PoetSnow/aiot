using NXAI.Shared.WebApi.Routing;

namespace Microsoft.AspNetCore.Mvc;

/// <summary>小程序 API 基类，路由 <c>api/portal/...</c>。</summary>
[ApiController]
[ApiExplorerSettings(GroupName = ApiSurfaces.PortalGroup)]
[Route(ApiSurfaces.PortalRouteTemplate)]
public abstract class PortalApiController : NXAIControllerBase;
