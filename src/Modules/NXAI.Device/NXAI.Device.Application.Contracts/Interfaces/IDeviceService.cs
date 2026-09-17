using NXAI.Device.Application.Contracts.Dtos;
using NXAI.Shared.Application.Contracts.Dtos;
using NXAI.Shared.Application.Contracts.ResultModels;

namespace NXAI.Device.Application.Contracts.Interfaces;

/// <summary>已绑定设备。Cooking 经本接口下发，不引用 MQTT。</summary>
public interface IDeviceService
{
    /// <summary>会员绑定 SN。未出库必须失败。</summary>
    Task<ServiceResult<IdDto>> BindAsync(long memberId, DeviceBindDto input);

    /// <summary>当前会员的设备列表。</summary>
    Task<List<DeviceDto>> GetByMemberAsync(long memberId);

    /// <summary>设备详情。memberId 有值时校验主人。</summary>
    Task<DeviceDto?> GetAsync(long id, long? memberId);

    /// <summary>按 SN 查设备。</summary>
    Task<DeviceDto?> GetBySnAsync(string sn);

    /// <summary>设备影子。</summary>
    Task<DeviceShadowDto?> GetShadowAsync(long id, long? memberId);

    /// <summary>仓位列表。</summary>
    Task<List<DeviceSlotDto>> GetSlotsAsync(long id, long? memberId);

    /// <summary>绑定仓位。ConsumableId 须属于该会员。</summary>
    Task<ServiceResult> BindSlotAsync(long id, string slotCode, long memberId, DeviceSlotBindingDto input);

    /// <summary>后台设备列表。</summary>
    Task<List<DeviceDto>> GetConsoleListAsync();

    /// <summary>设备激活，签发 X-Device-Token。</summary>
    Task<ServiceResult<DeviceTokenDto>> ActivateAsync(DeviceActivateDto input);

    /// <summary>轮换设备 Token。</summary>
    Task<ServiceResult<DeviceTokenDto>> RefreshTokenAsync(string currentToken);

    /// <summary>EMQX 登录校验。clientId=pot-{sn}，password=deviceToken。</summary>
    Task<bool> MqttAuthAsync(DeviceMqttAuthDto input);

    /// <summary>写入 Outbox。已 RECEIVED 的 commandId 拒绝当新投放。</summary>
    Task<ServiceResult> IssueCommandAsync(long deviceId, DeviceCommandIssueDto input);

    /// <summary>售后解绑。删除设备会话，不改 ast_sn。</summary>
    Task<ServiceResult> UnbindBySnAsync(string sn);

    /// <summary>一台设备同时最多一个未终态任务。</summary>
    Task<ServiceResult> TryLockTaskAsync(long deviceId, long taskId, int epoch);

    /// <summary>释放任务锁。</summary>
    Task ReleaseTaskAsync(long deviceId, long taskId);

    /// <summary>新任务开始前世代 +1。旧 epoch 包作废。</summary>
    Task<int> BumpEpochAsync(long deviceId);

    /// <summary>开发或 MQTT 把设备标为在线/离线。</summary>
    Task<ServiceResult> SetOnlineBySnAsync(string sn, bool online);

    /// <summary>写入能力快照。</summary>
    Task ApplyCapabilityAsync(string sn, string payloadJson);

    /// <summary>覆盖影子。禁止据此发投放。</summary>
    Task ApplyShadowAsync(string sn, DeviceShadowReportDto report);

    /// <summary>处理 ACK：Outbox RECEIVED，并发布进程内事件。</summary>
    Task ApplyAckAsync(string sn, DeviceAckDto ack);

    /// <summary>取出待发 Outbox，供 MQTT 工作器投递同一 commandId。</summary>
    Task<List<DeviceMqttPendingDto>> TakePendingOutboxAsync(int take);

    /// <summary>记录一次投递尝试。</summary>
    Task MarkOutboxAttemptAsync(long outboxId);
}
