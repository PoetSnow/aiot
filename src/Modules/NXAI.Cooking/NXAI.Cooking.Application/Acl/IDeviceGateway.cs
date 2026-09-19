using NXAI.Shared.Application.Contracts.ResultModels;

namespace NXAI.Cooking.Application.Acl;

/// <summary>设备防腐。下发只走这里，本模块不引用 MQTT。</summary>
public interface IDeviceGateway
{
    /// <summary>设备摘要。memberId 有值时校验主人。</summary>
    Task<CookingDeviceInfo?> GetAsync(long deviceId, long? memberId);

    /// <summary>是否在线。影子只展示，不当投料扳机。</summary>
    Task<bool> IsOnlineAsync(long deviceId, long memberId);

    /// <summary>仓位事实，用来把 TargetCode 解析到唯一仓。</summary>
    Task<List<CookingSlotInfo>> GetSlotsAsync(long deviceId, long memberId);

    /// <summary>新任务世代 +1。旧 epoch 包作废。</summary>
    Task<int> BumpEpochAsync(long deviceId);

    /// <summary>一台设备同时最多一个未终态任务。</summary>
    Task<ServiceResult> TryLockTaskAsync(long deviceId, long taskId, int epoch);

    /// <summary>释放任务锁。</summary>
    Task ReleaseTaskAsync(long deviceId, long taskId);

    /// <summary>写入 Device Outbox。已见过的 commandId 不得当新投放。</summary>
    Task<ServiceResult> IssueCommandAsync(long deviceId, long commandId, string payloadJson);
}

/// <summary>Cooking 看到的设备。</summary>
/// <param name="Id">设备 Id。</param>
/// <param name="DeviceSn">SN。</param>
/// <param name="ModelCode">型号。</param>
/// <param name="ActiveTaskId">在途任务。</param>
public sealed record CookingDeviceInfo(long Id, string DeviceSn, string ModelCode, long? ActiveTaskId);

/// <summary>Cooking 看到的仓位。</summary>
/// <param name="SlotCode">仓位码。</param>
/// <param name="SupportedModes">支持的投料模式。</param>
/// <param name="MaterialCode">物料编码。</param>
/// <param name="ConsumableId">耗材实例。</param>
public sealed record CookingSlotInfo(string SlotCode, string SupportedModes, string? MaterialCode, long? ConsumableId);
