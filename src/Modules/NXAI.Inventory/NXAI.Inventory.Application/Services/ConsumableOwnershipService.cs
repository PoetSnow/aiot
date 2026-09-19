using NXAI.Infra.Repository;
using NXAI.Inventory.Application.Contracts.Interfaces;
using ConsumableEntity = NXAI.Inventory.Repository.Entities.Consumable;

namespace NXAI.Inventory.Application.Services;

/// <summary>耗材归属。不调 Device，避免与绑仓形成构造环。</summary>
public sealed class ConsumableOwnershipService(IEfRepository<ConsumableEntity> consumables) : IConsumableOwnershipService
{
    public Task<bool> BelongsToMemberAsync(long consumableId, long memberId)
        => consumables.AnyAsync(x => x.Id == consumableId && x.MemberId == memberId);
}
