using NXAI.Inventory.Application.Contracts.Interfaces;

namespace NXAI.Device.Application.Acl;

/// <summary>转发 Inventory 契约。对方接口变化只改这里。</summary>
public sealed class InventoryGateway(IConsumableOwnershipService ownership) : IInventoryGateway
{
    public Task<bool> ConsumableBelongsToMemberAsync(long consumableId, long memberId)
        => ownership.BelongsToMemberAsync(consumableId, memberId);
}
