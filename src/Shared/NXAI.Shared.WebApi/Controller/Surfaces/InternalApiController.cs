using Microsoft.AspNetCore.Authorization;
using NXAI.Shared.WebApi.Routing;

namespace Microsoft.AspNetCore.Mvc;

/// <summary>内部调试 API 基类，路由 <c>api/internal/...</c>。尽量只在开发环境注册。</summary>
[ApiController]
[ApiExplorerSettings(GroupName = ApiSurfaces.InternalGroup)]
[Route(ApiSurfaces.InternalRouteTemplate)]
[AllowAnonymous]
public abstract class InternalApiController : NXAIControllerBase;
