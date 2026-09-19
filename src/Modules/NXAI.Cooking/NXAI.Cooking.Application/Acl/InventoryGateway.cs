using NXAI.Inventory.Application.Contracts.Interfaces;

namespace NXAI.Cooking.Application.Acl;

/// <summary>转发 Inventory 契约。扣减仍走 CAP，不走这里。</summary>
public sealed class InventoryGateway(IInventoryService inventory) : IInventoryGateway
{
    public Task<bool> HasEnoughAsync(long memberId, string typeCode, decimal qty)
        => inventory.HasEnoughAsync(memberId, typeCode, qty);
}
