using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NXAI.Member.Application.Contracts.Dtos;
using NXAI.Member.Application.Contracts.Interfaces;
using NXAI.Shared;
using NXAI.Shared.WebApi.Authentication.Bearer;
using NXAI.Shared.WebApi.Routing;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace NXAI.Member.API.Controllers;

/// <summary>小程序会员会话。</summary>
[Route($"{ApiSurfaces.PortalRoutePrefix}/auth")]
public sealed class MemberAuthController(IOptions<JWTOptions> jwtOptions, IMemberService memberService, UserContext userContext)
    : PortalApiController
{
    /// <summary>会员登录。</summary>
    /// <param name="input">手机号与短信验证码。</param>
    /// <returns>访问令牌与刷新令牌。</returns>
    [AllowAnonymous]
    [HttpPost("session")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<MemberTokenDto>> LoginAsync([FromBody] MemberLoginDto input)
    {
        var result = await memberService.LoginByMobileAsync(input);
        if (!result.IsSuccess)
        {
            return Problem(result.ProblemDetails);
        }

        return Created($"{ApiSurfaces.PortalRoutePrefix}/auth/session", CreateTokens(result.Content));
    }

    /// <summary>用刷新令牌换新的访问令牌。</summary>
    /// <param name="input">刷新令牌。</param>
    /// <returns>新的访问令牌与刷新令牌。</returns>
    [AllowAnonymous]
    [HttpPost("session/refresh")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<MemberTokenDto>> RefreshAsync([FromBody] MemberRefreshTokenDto input)
    {
        Claim? idClaim;
        try
        {
            idClaim = JwtTokenHelper.GetClaimFromRefeshToken(jwtOptions.Value, input.RefreshToken, JwtRegisteredClaimNames.NameId);
        }
        catch (SecurityTokenException)
        {
            return Forbid();
        }
        if (idClaim is null || !long.TryParse(idClaim.Value, out var memberId))
        {
            return Forbid();
        }

        var current = await memberService.GetValidatedInfoAsync(memberId);
        if (!current.IsSuccess)
        {
            return Forbid();
        }

        // jti 对不上说明刷新令牌已被轮换，旧令牌作废
        var jtiClaim = JwtTokenHelper.GetClaimFromRefeshToken(jwtOptions.Value, input.RefreshToken, JwtRegisteredClaimNames.Jti);
        if (jtiClaim is null || jtiClaim.Value != current.Content.ValidationVersion)
        {
            return Forbid();
        }

        var rotated = await memberService.RotateSessionAsync(memberId);
        if (!rotated.IsSuccess)
        {
            return Forbid();
        }

        return CreateTokens(rotated.Content);
    }

    /// <summary>当前登录会员的资料。</summary>
    /// <returns>会员资料；会话无效时 404。</returns>
    [HttpGet("profile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MemberProfileDto>> GetProfileAsync()
    {
        var profile = await memberService.GetProfileAsync(userContext.Id);
        return profile is null ? NotFound() : profile;
    }

    /// <summary>签发会员 JWT。token_type=member，aud=portal，只能打 /api/portal。</summary>
    private MemberTokenDto CreateTokens(MemberValidatedInfoDto member)
    {
        var accessToken = JwtTokenHelper.CreateAccessToken(
            jwtOptions.Value,
            member.ValidationVersion,
            member.Mobile,
            member.Id.ToString(),
            member.Nickname,
            string.Empty,
            BearerDefaults.Member,
            ApiSurfaces.PortalGroup);
        var refreshToken = JwtTokenHelper.CreateRefreshToken(jwtOptions.Value, member.ValidationVersion, member.Id.ToString());
        return new MemberTokenDto(accessToken.Token, accessToken.Expire, refreshToken.Token, refreshToken.Expire);
    }
}
