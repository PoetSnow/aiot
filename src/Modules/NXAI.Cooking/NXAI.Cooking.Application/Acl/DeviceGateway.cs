using NXAI.Device.Application.Contracts.Dtos;
using NXAI.Device.Application.Contracts.Interfaces;
using NXAI.Shared.Application.Contracts.ResultModels;

namespace NXAI.Cooking.Application.Acl;

/// <summary>转发 Device 契约。对方 DTO 变化只改这里。</summary>
public sealed class DeviceGateway(IDeviceService devices) : IDeviceGateway
{
    public async Task<CookingDeviceInfo?> GetAsync(long deviceId, long? memberId)
    {
        var dto = await devices.GetAsync(deviceId, memberId);
        return dto is null ? null : new CookingDeviceInfo(dto.Id, dto.DeviceSn, dto.ModelCode, dto.ActiveTaskId);
    }

    public async Task<bool> IsOnlineAsync(long deviceId, long memberId)
    {
        var shadow = await devices.GetShadowAsync(deviceId, memberId);
        return shadow is { Online: true };
    }

    public async Task<List<CookingSlotInfo>> GetSlotsAsync(long deviceId, long memberId)
    {
        var slots = await devices.GetSlotsAsync(deviceId, memberId);
        return slots.Select(s => new CookingSlotInfo(s.SlotCode, s.SupportedModes, s.MaterialCode, s.ConsumableId)).ToList();
    }

    public Task<int> BumpEpochAsync(long deviceId) => devices.BumpEpochAsync(deviceId);

    public Task<ServiceResult> TryLockTaskAsync(long deviceId, long taskId, int epoch)
        => devices.TryLockTaskAsync(deviceId, taskId, epoch);

    public Task ReleaseTaskAsync(long deviceId, long taskId) => devices.ReleaseTaskAsync(deviceId, taskId);

    public Task<ServiceResult> IssueCommandAsync(long deviceId, long commandId, string payloadJson)
        => devices.IssueCommandAsync(deviceId, new DeviceCommandIssueDto
        {
            CommandId = commandId,
            PayloadJson = payloadJson
        });
}
