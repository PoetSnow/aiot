using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NXAI.Shared;
using NXAI.Shared.WebApi.Routing;

namespace NXAI.System.API.Controllers;

/// <summary>后台鉴权探活。员工令牌应 200，会员令牌应 403。</summary>
[Route($"{ApiSurfaces.ConsoleRoutePrefix}/auth/probe")]
public sealed class AuthProbeController(UserContext userContext) : ConsoleApiController
{
    /// <summary>返回当前员工会话的表面与 token_type。</summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Get() => Ok(new { surface = ApiSurfaces.ConsoleGroup, userContext.Id, tokenType = userContext.TokenType });
}
