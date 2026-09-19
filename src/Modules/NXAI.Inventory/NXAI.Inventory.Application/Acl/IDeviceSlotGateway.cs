using NXAI.Shared.Application.Contracts.ResultModels;

namespace NXAI.Inventory.Application.Acl;

/// <summary>仓位防腐。放置先申请绑仓，成功后再写本模块投影。</summary>
public interface IDeviceSlotGateway
{
    /// <summary>把耗材实例绑到指定仓。ConsumableId 须属于该会员。</summary>
    Task<ServiceResult> BindAsync(long deviceId, string slotCode, long memberId, long consumableId, string materialCode);
}
