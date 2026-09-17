using NXAI.Shared.WebApi.Routing;

namespace Microsoft.AspNetCore.Mvc;

/// <summary>后台管理 API 基类，路由 <c>api/console/...</c>。</summary>
[ApiController]
[ApiExplorerSettings(GroupName = ApiSurfaces.ConsoleGroup)]
[Route(ApiSurfaces.ConsoleRouteTemplate)]
public abstract class ConsoleApiController : NXAIControllerBase;
