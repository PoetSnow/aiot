using NXAI.Shared.Application.Contracts.Dtos;

namespace NXAI.Member.Application.Contracts.Dtos;

/// <summary>刷新会员访问令牌的参数。</summary>
public class MemberRefreshTokenDto : InputDto
{
    /// <summary>登录时下发的刷新令牌。</summary>
    public string RefreshToken { get; set; } = string.Empty;
}
