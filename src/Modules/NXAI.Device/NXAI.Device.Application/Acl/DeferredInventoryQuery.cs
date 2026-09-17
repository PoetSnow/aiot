using NXAI.Device.Application.Contracts.Interfaces;

namespace NXAI.Device.Application.Acl;

/// <summary>Inventory 未接入时的占位：任何 ConsumableId 都视为不属于会员。</summary>
public sealed class DeferredInventoryQuery : IInventoryQuery
{
    public Task<bool> ConsumableBelongsToMemberAsync(long consumableId, long memberId) => Task.FromResult(false);
}
