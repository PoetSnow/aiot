namespace NXAI.Shared.WebApi.Authentication.Bearer;

/// <summary>JWT 自定义声明名与 token_type 取值。</summary>
public static class BearerDefaults
{
    /// <summary>角色 Id 列表声明。</summary>
    public const string RoleIds = "roleids";

    /// <summary>旧登录者类型声明，兼容 manager / customer。</summary>
    public const string LoginerType = "loginer";

    /// <summary>令牌类型声明：staff 或 member。</summary>
    public const string TokenType = "token_type";

    /// <summary>旧员工取值，归一成 staff。</summary>
    public const string Manager = "manager";

    /// <summary>旧会员取值，归一成 member。</summary>
    public const string Customer = "customer";

    /// <summary>员工令牌，只能打 /api/console。</summary>
    public const string Staff = "staff";

    /// <summary>会员令牌，只能打 /api/portal。</summary>
    public const string Member = "member";

    /// <summary>把旧 loginer 取值归一成 staff / member。</summary>
    public static string NormalizeTokenType(string? value) => value switch
    {
        Manager => Staff,
        Customer => Member,
        _ => value ?? string.Empty
    };
}
