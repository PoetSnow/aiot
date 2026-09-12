using NXAI.Shared.WebApi.Routing;

namespace Microsoft.AspNetCore.Mvc;

/// <summary>
/// Base controller for mini-program APIs (<c>api/portal/...</c>).
/// </summary>
[ApiController]
[ApiExplorerSettings(GroupName = ApiSurfaces.PortalGroup)]
[Route(ApiSurfaces.PortalRouteTemplate)]
public abstract class PortalApiController : NXAIControllerBase;
