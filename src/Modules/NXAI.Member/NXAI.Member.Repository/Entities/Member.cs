using NXAI.Infra.Repository;

namespace NXAI.Member.Repository.Entities;

/// <summary>小程序会员，表 mb_member。Id 与 sys_user 不是同一空间。</summary>
public class Member : EfEntity
{
    /// <summary>手机号最大长度。</summary>
    public const int Mobile_MaxLength = 16;

    /// <summary>微信 OpenId 最大长度。</summary>
    public const int OpenId_MaxLength = 64;

    /// <summary>微信 UnionId 最大长度。</summary>
    public const int UnionId_MaxLength = 64;

    /// <summary>昵称最大长度。</summary>
    public const int Nickname_MaxLength = 32;

    /// <summary>头像地址最大长度。</summary>
    public const int Avatar_MaxLength = 256;

    /// <summary>手机号，有值则唯一。</summary>
    public string? Mobile { get; set; }

    /// <summary>微信 OpenId，有值则唯一。</summary>
    public string? OpenId { get; set; }

    /// <summary>微信 UnionId。</summary>
    public string? UnionId { get; set; }

    /// <summary>昵称。</summary>
    public string Nickname { get; set; } = string.Empty;

    /// <summary>头像地址。</summary>
    public string Avatar { get; set; } = string.Empty;

    /// <summary>1 正常 / 0 禁用 / 2 注销。</summary>
    public int Status { get; set; } = MemberStatus.Normal;

    /// <summary>最近一次登录时间。</summary>
    public DateTime? LastLoginTime { get; set; }
}

/// <summary>会员账号状态。</summary>
public static class MemberStatus
{
    /// <summary>禁用，不可登录。</summary>
    public const int Disabled = 0;

    /// <summary>正常。</summary>
    public const int Normal = 1;

    /// <summary>已注销。</summary>
    public const int Cancelled = 2;
}
