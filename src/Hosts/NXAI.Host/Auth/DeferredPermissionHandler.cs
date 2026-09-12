using NXAI.Shared.WebApi.Authorization.Handlers;

namespace NXAI.Host.Auth;

/// <summary>第 1 步占位：不读 Redis 权限缓存。带 <c>AdncAuthorize</c> 的接口在后续步骤才生效。</summary>
internal sealed class DeferredPermissionHandler : AbstractPermissionHandler
{
    protected override Task<bool> CheckUserPermissions(long userId, IEnumerable<string> requestPermissions, string userBelongsRoleIds)
        => Task.FromResult(true);
}
