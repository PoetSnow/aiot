using NXAI.Shared.WebApi.Routing;

namespace Microsoft.AspNetCore.Mvc;

/// <summary>
/// Base controller for admin-console APIs (<c>api/console/...</c>).
/// </summary>
[ApiController]
[ApiExplorerSettings(GroupName = ApiSurfaces.ConsoleGroup)]
[Route(ApiSurfaces.ConsoleRouteTemplate)]
public abstract class ConsoleApiController : NXAIControllerBase;
