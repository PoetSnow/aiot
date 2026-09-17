using NXAI.Shared.Application.Contracts.Dtos;

namespace NXAI.Member.Application.Contracts.Dtos;

/// <summary>会员访问令牌与刷新令牌。</summary>
/// <param name="Token">访问令牌。</param>
/// <param name="Expire">访问令牌过期时间。</param>
/// <param name="RefreshToken">刷新令牌。</param>
/// <param name="RefreshExpire">刷新令牌过期时间。</param>
public record MemberTokenDto(string Token, DateTime Expire, string RefreshToken, DateTime RefreshExpire) : IDto;
