using NXAI.Shared.Application.Contracts.Dtos;

namespace NXAI.Member.Application.Contracts.Dtos;

/// <summary>签发会员 JWT 所需的校验信息。</summary>
/// <param name="Id">会员 Id。</param>
/// <param name="Mobile">手机号，写入 unique_name。</param>
/// <param name="Nickname">昵称。</param>
/// <param name="ValidationVersion">会话版本，与 JWT jti 对齐。</param>
/// <param name="Status">账号是否可用。</param>
public record MemberValidatedInfoDto(long Id, string Mobile, string Nickname, string ValidationVersion, bool Status) : IDto;
