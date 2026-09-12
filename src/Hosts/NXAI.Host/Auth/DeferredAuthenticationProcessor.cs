using NXAI.Shared.WebApi.Authentication.Processors;

namespace NXAI.Host.Auth;

/// <summary>第 1 步占位：不读 Redis。会员/员工会话校验在第 2 步接入。</summary>
internal sealed class DeferredAuthenticationProcessor : AbstractAuthenticationProcessor
{
    protected override Task<(string? ValidationVersion, bool Status)> GetValidatedInfoAsync(long userId)
        => Task.FromResult<(string?, bool)>((null, false));
}
