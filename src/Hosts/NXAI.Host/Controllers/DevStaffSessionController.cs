using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NXAI.Shared.WebApi.Authentication.Bearer;
using NXAI.Shared.WebApi.Routing;

namespace NXAI.Host.Controllers;

/// <summary>开发用：不连 System 库签发员工令牌，用来验三端隔离。</summary>
[Route(ApiSurfaces.InternalRoutePrefix)]
public sealed class DevStaffSessionController(IOptions<JWTOptions> jwtOptions) : InternalApiController
{
    /// <summary>签发员工访问令牌。token_type=staff，aud=console。</summary>
    [HttpPost("dev/staff-session")]
    public IActionResult Create()
    {
        var jti = JwtTokenHelper.GenerateJti();
        var accessToken = JwtTokenHelper.CreateAccessToken(
            jwtOptions.Value,
            jti,
            "dev-staff",
            "1000000000001",
            "dev-staff",
            string.Empty,
            BearerDefaults.Staff,
            ApiSurfaces.ConsoleGroup);
        var refreshToken = JwtTokenHelper.CreateRefreshToken(jwtOptions.Value, jti, "1000000000001");
        return Ok(new
        {
            token = accessToken.Token,
            expire = accessToken.Expire,
            refreshToken = refreshToken.Token,
            refreshExpire = refreshToken.Expire
        });
    }
}
