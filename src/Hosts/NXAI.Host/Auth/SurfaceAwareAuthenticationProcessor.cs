using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using NXAI.Member.Application.Contracts.Interfaces;
using NXAI.Shared.WebApi.Authentication.Bearer;
using NXAI.Shared.WebApi.Authentication.Processors;

namespace NXAI.Host.Auth;

/// <summary>会员走 mb_member 会话；员工暂用 JWT jti，不查 sys_user。</summary>
internal sealed class SurfaceAwareAuthenticationProcessor(IMemberService memberService) : AbstractAuthenticationProcessor
{
    protected override async Task<(string? ValidationVersion, bool Status)> GetValidatedInfoAsync(long userId, ClaimsPrincipal claimsPrincipal)
    {
        var tokenType = BearerDefaults.NormalizeTokenType(
            claimsPrincipal.FindFirst(BearerDefaults.TokenType)?.Value
            ?? claimsPrincipal.FindFirst(BearerDefaults.LoginerType)?.Value);
        var jti = claimsPrincipal.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

        if (string.Equals(tokenType, BearerDefaults.Member, StringComparison.Ordinal))
        {
            return await memberService.GetValidatedSessionAsync(userId);
        }

        return (jti, !string.IsNullOrWhiteSpace(jti));
    }
}
