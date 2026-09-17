using NXAI.Member.Application.Contracts.Dtos;
using NXAI.Shared.Application.Contracts.ResultModels;

namespace NXAI.Member.Application.Contracts.Interfaces;

/// <summary>小程序会员账号。不读写 sys_user。</summary>
public interface IMemberService
{
    /// <summary>手机号登录，不存在则注册。</summary>
    /// <param name="input">手机号与短信验证码。</param>
    /// <returns>用于签发 JWT 的校验信息。</returns>
    Task<ServiceResult<MemberValidatedInfoDto>> LoginByMobileAsync(MemberLoginDto input);

    /// <summary>按会员 Id 读取当前会话校验信息。</summary>
    /// <param name="memberId">会员 Id。</param>
    Task<ServiceResult<MemberValidatedInfoDto>> GetValidatedInfoAsync(long memberId);

    /// <summary>轮换会话版本，使旧访问令牌失效。</summary>
    /// <param name="memberId">会员 Id。</param>
    Task<ServiceResult<MemberValidatedInfoDto>> RotateSessionAsync(long memberId);

    /// <summary>读取会员资料。</summary>
    /// <param name="memberId">会员 Id。</param>
    Task<MemberProfileDto?> GetProfileAsync(long memberId);

    /// <summary>鉴权时核对会员会话是否仍有效。</summary>
    /// <param name="memberId">会员 Id。</param>
    Task<(string? ValidationVersion, bool Active)> GetValidatedSessionAsync(long memberId);

    /// <summary>会员是否存在。</summary>
    /// <param name="memberId">会员 Id。</param>
    Task<bool> ExistsAsync(long memberId);

    /// <summary>会员是否已注销。</summary>
    /// <param name="memberId">会员 Id。</param>
    Task<bool> IsCancelledAsync(long memberId);
}
