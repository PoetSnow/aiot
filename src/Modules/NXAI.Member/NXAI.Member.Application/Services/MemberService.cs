using System.Net;
using System.Text.RegularExpressions;
using NXAI.Infra.IdGenerater.Yitter;
using NXAI.Infra.Repository;
using NXAI.Member.Application.Contracts.Dtos;
using NXAI.Member.Application.Contracts.Interfaces;
using NXAI.Member.Application.Stores;
using NXAI.Shared.Application.Contracts.ResultModels;
using MemberEntity = NXAI.Member.Repository.Entities.Member;
using MemberStatus = NXAI.Member.Repository.Entities.MemberStatus;

namespace NXAI.Member.Application.Services;

/// <summary>小程序会员账号。实现见 <see cref="IMemberService"/>。</summary>
public sealed class MemberService(IEfRepository<MemberEntity> members, InMemoryMemberSessionStore sessions) : IMemberService
{
    /// <summary>开发环境短信验证码，未接短信通道前固定此值。</summary>
    public const string DevSmsCode = "000000";

    public async Task<ServiceResult<MemberValidatedInfoDto>> LoginByMobileAsync(MemberLoginDto input)
    {
        var mobile = input.Mobile?.Trim() ?? string.Empty;
        if (!Regex.IsMatch(mobile, @"^\d{11}$"))
        { 

            return new ProblemDetails(HttpStatusCode.BadRequest, "手机号格式不正确");
        }

        if (!string.Equals(input.SmsCode?.Trim(), DevSmsCode, StringComparison.Ordinal))
        {
            return new ProblemDetails(HttpStatusCode.BadRequest, "短信验证码错误");
        }

        var member = await members.FetchAsync(x => x.Mobile == mobile, noTracking: false);
        if (member is null)
        {
            member = new MemberEntity
            {
                // 会员 Id 与 sys_user 不是同一空间，禁止拿员工 Id 写入
                Id = IdGenerater.GetNextId(),
                Mobile = mobile,
                Nickname = $"用户{mobile[^4..]}",
                Status = MemberStatus.Normal,
                LastLoginTime = DateTime.Now
            };
            await members.InsertAsync(member);
        }
        else
        {
            if (member.Status != MemberStatus.Normal)
            {
                return new ProblemDetails(HttpStatusCode.Forbidden, "账号不可用");
            }

            member.LastLoginTime = DateTime.Now;
            await members.UpdateAsync(member);
        }

        return ServiceResult(OpenSession(member));
    }

    public async Task<ServiceResult<MemberValidatedInfoDto>> GetValidatedInfoAsync(long memberId)
    {
        var member = await members.FetchAsync(x => x.Id == memberId);
        if (member is null || member.Status != MemberStatus.Normal || !sessions.TryGet(memberId, out var session) || !session.Active)
        {
            return new ProblemDetails(HttpStatusCode.Forbidden, "会话无效");
        }

        return ServiceResult(new MemberValidatedInfoDto(member.Id, member.Mobile ?? string.Empty, member.Nickname, session.Jti, true));
    }

    public async Task<ServiceResult<MemberValidatedInfoDto>> RotateSessionAsync(long memberId)
    {
        var member = await members.FetchAsync(x => x.Id == memberId);
        if (member is null || member.Status != MemberStatus.Normal)
        {
            return new ProblemDetails(HttpStatusCode.Forbidden, "会话无效");
        }

        return ServiceResult(OpenSession(member));
    }

    public async Task<MemberProfileDto?> GetProfileAsync(long memberId)
    {
        var member = await members.FetchAsync(x => x.Id == memberId);
        return member is null
            ? null
            : new MemberProfileDto(member.Id, member.Mobile, member.Nickname, member.Avatar, member.Status);
    }

    public async Task<(string? ValidationVersion, bool Active)> GetValidatedSessionAsync(long memberId)
    {
        if (!sessions.TryGet(memberId, out var session))
        {
            return (null, false);
        }

        var member = await members.FetchAsync(x => x.Id == memberId);
        var active = session.Active && member is { Status: MemberStatus.Normal };
        return (session.Jti, active);
    }

    public Task<bool> ExistsAsync(long memberId) => members.AnyAsync(x => x.Id == memberId);

    public async Task<bool> IsCancelledAsync(long memberId)
    {
        var member = await members.FetchAsync(x => x.Id == memberId);
        return member?.Status == MemberStatus.Cancelled;
    }

    /// <summary>新开会话：jti 写入内存，旧令牌随即作废。</summary>
    private MemberValidatedInfoDto OpenSession(MemberEntity member)
    {
        var jti = Guid.NewGuid().ToString("N");
        sessions.Set(member.Id, jti, true);
        return new MemberValidatedInfoDto(member.Id, member.Mobile ?? string.Empty, member.Nickname, jti, true);
    }

    private static ServiceResult<MemberValidatedInfoDto> ServiceResult(MemberValidatedInfoDto value) => new(value);
}
