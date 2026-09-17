using NXAI.Device.Application.Contracts.Interfaces;
using NXAI.Infra.Repository;
using ConsumableEntity = NXAI.Inventory.Repository.Entities.Consumable;

namespace NXAI.Inventory.Application.Acl;

/// <summary>覆盖 Device 空查询，允许仓位绑定耗材实例。</summary>
public sealed class InventoryQueryAdapter(IEfRepository<ConsumableEntity> consumables) : IInventoryQuery
{
    public async Task<bool> ConsumableBelongsToMemberAsync(long consumableId, long memberId)
    {
        return await consumables.AnyAsync(x => x.Id == consumableId && x.MemberId == memberId);
    }
}
