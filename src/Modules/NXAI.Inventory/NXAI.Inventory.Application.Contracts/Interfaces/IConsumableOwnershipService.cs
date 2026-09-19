namespace NXAI.Inventory.Application.Contracts.Interfaces;

/// <summary>耗材归属只读。Device 绑仓经 ACL 调用，本接口不依赖 Device。</summary>
public interface IConsumableOwnershipService
{
    /// <summary>耗材实例是否属于该会员。</summary>
    Task<bool> BelongsToMemberAsync(long consumableId, long memberId);
}
