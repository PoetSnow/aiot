using NXAI.Shared.Application.Contracts.Dtos;

namespace NXAI.Member.Application.Contracts.Dtos;

/// <summary>会员登录参数。</summary>
public class MemberLoginDto : InputDto
{
    /// <summary>11 位手机号。</summary>
    public string Mobile { get; set; } = string.Empty;

    /// <summary>短信验证码。开发环境固定 000000。</summary>
    public string SmsCode { get; set; } = string.Empty;
}
