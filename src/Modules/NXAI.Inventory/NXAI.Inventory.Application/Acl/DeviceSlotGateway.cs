using NXAI.Device.Application.Contracts.Dtos;
using NXAI.Device.Application.Contracts.Interfaces;
using NXAI.Shared.Application.Contracts.ResultModels;

namespace NXAI.Inventory.Application.Acl;

/// <summary>转发 Device 契约。对方 DTO 变化只改这里。</summary>
public sealed class DeviceSlotGateway(IDeviceService devices) : IDeviceSlotGateway
{
    public Task<ServiceResult> BindAsync(long deviceId, string slotCode, long memberId, long consumableId, string materialCode)
        => devices.BindSlotAsync(deviceId, slotCode, memberId, new DeviceSlotBindingDto
        {
            ConsumableId = consumableId,
            MaterialCode = materialCode
        });
}
