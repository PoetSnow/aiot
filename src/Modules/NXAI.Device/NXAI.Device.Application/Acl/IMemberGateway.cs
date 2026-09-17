namespace NXAI.Device.Application.Acl;

/// <summary>会员防腐。绑定前确认未注销。</summary>
public interface IMemberGateway
{
    /// <summary>会员是否存在且未注销。</summary>
    Task<bool> ExistsActiveAsync(long memberId);
}
