using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NXAI.Shared.WebApi.Authentication.Bearer;

public static class JwtTokenHelper
{
    public static string GenerateJti() => Guid.NewGuid().ToString("N");

    /// <summary>
    /// create access token
    /// </summary>
    /// <param name="jwtConfig"></param>
    /// <param name="jti"></param>
    /// <param name="uniqueName"></param>
    /// <param name="nameId"></param>
    /// <param name="name"></param>
    /// <param name="roleIds"></param>
    /// <param name="loginerType"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static JwtToken CreateAccessToken(
        JWTOptions jwtConfig
        , string jti
        , string uniqueName
        , string nameId
        , string name
        , string roleIds
        , string loginerType
        , string? audience = null)
    {
        if (jti.IsNullOrWhiteSpace())
        {
            throw new ArgumentNullException(nameof(jti));
        }

        var tokenType = BearerDefaults.NormalizeTokenType(loginerType);
        var claims = new Claim[]
        {
            new(JwtRegisteredClaimNames.Jti, jti),
            new(JwtRegisteredClaimNames.Sub, nameId),
            new(JwtRegisteredClaimNames.UniqueName, uniqueName),
            new(JwtRegisteredClaimNames.NameId, nameId),
            new(JwtRegisteredClaimNames.Name, name),
            new(BearerDefaults.RoleIds, roleIds),
            new(BearerDefaults.LoginerType, tokenType),
            new(BearerDefaults.TokenType, tokenType)
        };
        return WriteToken(jwtConfig, claims, Tokens.AccessToken, audience);
    }

    /// <summary>
    ///  create refresh token
    /// </summary>
    /// <param name="jwtConfig"></param>
    /// <param name="jti"></param>
    /// <param name="nameId"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static JwtToken CreateRefreshToken(
        JWTOptions jwtConfig
        , string jti
        , string nameId
        )
    {
        if (jti.IsNullOrWhiteSpace())
        {
            throw new ArgumentNullException(nameof(jti));
        }

        var claims = new Claim[]
        {
            new(JwtRegisteredClaimNames.Jti,jti),
            new(JwtRegisteredClaimNames.NameId, nameId),
        };
        return WriteToken(jwtConfig, claims, Tokens.RefreshToken, audience: null);
    }

    /// <summary>
    /// get claim from refesh token
    /// </summary>
    /// <param name="jwtConfig"></param>
    /// <param name="refreshToken"></param>
    /// <param name="claimName"></param>
    /// <returns></returns>
    public static Claim? GetClaimFromRefeshToken(JWTOptions jwtConfig, string refreshToken, string claimName)
    {
        try
        {
            var parameters = jwtConfig.GenarateTokenValidationParameters();
            parameters.RequireExpirationTime = false;
            parameters.ValidateLifetime = false;
            var tokenHandler = new JwtSecurityTokenHandler { MapInboundClaims = false };
            var result = tokenHandler.ValidateToken(refreshToken, parameters, out _);
            if (result.Identity is null || !result.Identity.IsAuthenticated)
            {
                return null;
            }

            return result.Claims.FirstOrDefault(x => x.Type == claimName);
        }
        catch (SecurityTokenException)
        {
            return null;
        }
    }

    /// <summary>
    ///  write token
    /// </summary>
    /// <param name="jwtConfig"></param>
    /// <param name="claims"></param>
    /// <param name="tokenType"></param>
    /// <returns></returns>
    private static JwtToken WriteToken(JWTOptions jwtConfig, Claim[] claims, Tokens tokenType, string? audience)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SymmetricSecurityKey));

        var issuer = jwtConfig.ValidIssuer;
        audience ??= tokenType.Equals(Tokens.AccessToken) ? jwtConfig.ValidAudience : jwtConfig.RefreshTokenAudience;
        var seconds = tokenType.Equals(Tokens.AccessToken) ? jwtConfig.Expire : jwtConfig.RefreshTokenExpire;
        var expires = DateTime.Now.AddSeconds(seconds);
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            notBefore: DateTime.Now,
            expires: expires,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );
        return new JwtToken(new JwtSecurityTokenHandler().WriteToken(token), expires);
    }
}
