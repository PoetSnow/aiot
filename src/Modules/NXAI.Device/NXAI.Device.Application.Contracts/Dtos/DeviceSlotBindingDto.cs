using NXAI.Shared.Application.Contracts.Dtos;

namespace NXAI.Device.Application.Contracts.Dtos;

/// <summary>绑定仓位。Inventory 未接入前不要传 ConsumableId。</summary>
public class DeviceSlotBindingDto : InputDto
{
    /// <summary>物料编码。可与耗材二选一。</summary>
    public string? MaterialCode { get; set; }

    /// <summary>耗材实例 Id。须属于该会员。</summary>
    public long? ConsumableId { get; set; }
}
