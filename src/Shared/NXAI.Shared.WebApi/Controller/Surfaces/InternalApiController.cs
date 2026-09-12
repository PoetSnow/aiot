using Microsoft.AspNetCore.Authorization;
using NXAI.Shared.WebApi.Routing;

namespace Microsoft.AspNetCore.Mvc;

/// <summary>
/// Base controller for internal debug APIs (<c>api/internal/...</c>). Register controllers only in Development when possible.
/// </summary>
[ApiController]
[ApiExplorerSettings(GroupName = ApiSurfaces.InternalGroup)]
[Route(ApiSurfaces.InternalRouteTemplate)]
[AllowAnonymous]
public abstract class InternalApiController : NXAIControllerBase;
