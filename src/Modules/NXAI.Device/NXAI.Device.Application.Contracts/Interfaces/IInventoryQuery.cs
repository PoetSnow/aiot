namespace NXAI.Device.Application.Contracts.Interfaces;

/// <summary>耗材查询。Inventory 后注册覆盖空实现。</summary>
public interface IInventoryQuery
{
    /// <summary>耗材实例是否属于该会员。</summary>
    Task<bool> ConsumableBelongsToMemberAsync(long consumableId, long memberId);
}
