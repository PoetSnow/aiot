using System.Net;
using Microsoft.EntityFrameworkCore;
using NXAI.Infra.IdGenerater.Yitter;
using NXAI.Infra.Repository;
using NXAI.Inventory.Application.Acl;
using NXAI.Inventory.Application.Contracts.Dtos;
using NXAI.Inventory.Application.Contracts.Interfaces;
using NXAI.Shared.Application.Contracts.Dtos;
using NXAI.Shared.Application.Contracts.ResultModels;
using ConsumableEntity = NXAI.Inventory.Repository.Entities.Consumable;
using ConsumableStatus = NXAI.Inventory.Repository.Entities.ConsumableStatus;

namespace NXAI.Inventory.Application.Services;

/// <summary>耗材实例。实现见 <see cref="IInventoryService"/>。</summary>
public sealed class InventoryService(
    IEfRepository<ConsumableEntity> consumables,
    IDeviceSlotGateway slots) : IInventoryService
{
    public async Task<ServiceResult<IdDto>> CreateAsync(long memberId, ConsumableCreationDto input)
    {
        var type = input.ConsumableTypeCode?.Trim() ?? string.Empty;
        if (type.Length == 0)
        {
            return new ProblemDetails(HttpStatusCode.BadRequest, "耗材类型不能为空");
        }

        var qty = input.Qty <= 0 ? 1 : input.Qty;
        var entity = new ConsumableEntity
        {
            Id = IdGenerater.GetNextId(),
            MemberId = memberId,
            ConsumableTypeCode = type,
            ProductId = input.ProductId,
            Form = string.IsNullOrWhiteSpace(input.Form) ? "PACKAGE" : input.Form.Trim(),
            TotalQty = qty,
            RemainQty = qty,
            QtyUnit = string.IsNullOrWhiteSpace(input.QtyUnit) ? "PACK" : input.QtyUnit.Trim(),
            Status = ConsumableStatus.Available
        };
        await consumables.InsertAsync(entity);
        return new IdDto(entity.Id);
    }

    public async Task<List<ConsumableDto>> GetByMemberAsync(long memberId)
    {
        var list = await consumables.GetAll().Where(x => x.MemberId == memberId).OrderByDescending(x => x.Id).ToListAsync();
        return list.Select(Map).ToList();
    }

    public async Task<ServiceResult> PlaceAsync(long memberId, long id, ConsumablePlaceDto input)
    {
        var entity = await consumables.FetchAsync(x => x.Id == id, noTracking: false);
        if (entity is null || entity.MemberId != memberId)
        {
            return new ProblemDetails(HttpStatusCode.NotFound, "耗材不存在");
        }

        var slot = input.SlotCode?.Trim() ?? string.Empty;
        if (input.DeviceId <= 0 || slot.Length == 0)
        {
            return new ProblemDetails(HttpStatusCode.BadRequest, "设备与仓位不能为空");
        }

        var bind = await slots.BindAsync(input.DeviceId, slot, memberId, entity.Id, entity.ConsumableTypeCode);
        if (!bind.IsSuccess)
        {
            return bind;
        }

        entity.PlacedDeviceId = input.DeviceId;
        entity.PlacedSlotCode = slot;
        await consumables.UpdateAsync(entity);
        return new ServiceResult();
    }

    public async Task<bool> HasEnoughAsync(long memberId, string typeCode, decimal qty)
    {
        var code = typeCode?.Trim() ?? string.Empty;
        var remain = await consumables.GetAll()
            .Where(x => x.MemberId == memberId && x.ConsumableTypeCode == code && x.Status == ConsumableStatus.Available)
            .SumAsync(x => (decimal?)x.RemainQty) ?? 0;
        return remain >= qty;
    }

    public async Task DecrementAsync(IReadOnlyList<long> consumableIds)
    {
        if (consumableIds.Count == 0)
        {
            return;
        }

        var list = await consumables.GetAll(noTracking: false).Where(x => consumableIds.Contains(x.Id)).ToListAsync();
        foreach (var item in list)
        {
            if (item.RemainQty <= 0)
            {
                continue;
            }

            item.RemainQty -= 1;
            if (item.RemainQty <= 0)
            {
                item.Status = ConsumableStatus.Empty;
            }

            await consumables.UpdateAsync(item);
        }
    }

    private static ConsumableDto Map(ConsumableEntity x) => new()
    {
        Id = x.Id,
        ConsumableTypeCode = x.ConsumableTypeCode,
        Form = x.Form,
        RemainQty = x.RemainQty,
        QtyUnit = x.QtyUnit,
        Status = x.Status,
        PlacedDeviceId = x.PlacedDeviceId,
        PlacedSlotCode = x.PlacedSlotCode
    };
}
