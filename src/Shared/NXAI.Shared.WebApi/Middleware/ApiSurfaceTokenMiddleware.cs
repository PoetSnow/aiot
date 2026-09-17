using Microsoft.AspNetCore.Http;
using NXAI.Shared.WebApi.Authentication.Bearer;
using NXAI.Shared.WebApi.Routing;

namespace NXAI.Shared.WebApi.Middleware;

/// <summary>员工令牌禁止打 /api/portal，会员令牌禁止打 /api/console。</summary>
public sealed class ApiSurfaceTokenMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var tokenType = BearerDefaults.NormalizeTokenType(
                context.User.FindFirst(BearerDefaults.TokenType)?.Value
                ?? context.User.FindFirst(BearerDefaults.LoginerType)?.Value);
            var path = context.Request.Path.Value ?? string.Empty;

            if (path.StartsWith($"/{ApiSurfaces.ConsoleRoutePrefix}", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(tokenType, BearerDefaults.Staff, StringComparison.Ordinal))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return;
            }

            if (path.StartsWith($"/{ApiSurfaces.PortalRoutePrefix}", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(tokenType, BearerDefaults.Member, StringComparison.Ordinal))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return;
            }
        }

        await next(context);
    }
}
