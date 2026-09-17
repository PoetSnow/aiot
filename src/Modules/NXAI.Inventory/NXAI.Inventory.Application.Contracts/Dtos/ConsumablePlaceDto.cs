using NXAI.Shared.Application.Contracts.Dtos;

namespace NXAI.Inventory.Application.Contracts.Dtos;

/// <summary>把耗材放到设备仓位。</summary>
public class ConsumablePlaceDto : InputDto
{
    /// <summary>设备 Id。</summary>
    public long DeviceId { get; set; }

    /// <summary>仓位码，如 S3。</summary>
    public string SlotCode { get; set; } = string.Empty;
}
