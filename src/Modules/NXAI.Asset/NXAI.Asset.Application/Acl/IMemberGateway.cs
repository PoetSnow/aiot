namespace NXAI.Asset.Application.Acl;

/// <summary>会员防腐。出库挂会员时确认会员存在，不直接碰 mb_member。</summary>
public interface IMemberGateway
{
    /// <summary>会员是否存在且未注销。</summary>
    Task<bool> ExistsActiveAsync(long memberId);
}
