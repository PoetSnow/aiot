namespace NXAI.Device.Application.Acl;

/// <summary>耗材防腐。绑仓只问「属于该会员」，不碰 inv_ 表。</summary>
public interface IInventoryGateway
{
    /// <summary>耗材实例是否属于该会员。</summary>
    Task<bool> ConsumableBelongsToMemberAsync(long consumableId, long memberId);
}
