using NXAI.Shared.Application.Contracts.Dtos;

namespace NXAI.Member.Application.Contracts.Dtos;

/// <summary>当前登录会员的资料。</summary>
/// <param name="Id">会员 Id，与 sys_user.Id 不是同一空间。</param>
/// <param name="Mobile">手机号。</param>
/// <param name="Nickname">昵称。</param>
/// <param name="Avatar">头像地址。</param>
/// <param name="Status">1 正常 / 0 禁用 / 2 注销。</param>
public record MemberProfileDto(long Id, string? Mobile, string Nickname, string Avatar, int Status) : IDto;
