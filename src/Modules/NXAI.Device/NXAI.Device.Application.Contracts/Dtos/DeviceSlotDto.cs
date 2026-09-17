using NXAI.Shared.Application.Contracts.Dtos;

namespace NXAI.Device.Application.Contracts.Dtos;

/// <summary>仓位事实。</summary>
/// <param name="SlotCode">仓位码。</param>
/// <param name="SlotType">仓位类型。</param>
/// <param name="SupportedModes">支持的投料模式。</param>
/// <param name="BindingKind">0 空 / 1 物料 / 2 耗材。</param>
/// <param name="MaterialCode">物料编码。</param>
/// <param name="ConsumableId">耗材实例。</param>
public record DeviceSlotDto(
    string SlotCode,
    string SlotType,
    string SupportedModes,
    int BindingKind,
    string? MaterialCode,
    long? ConsumableId) : IDto;
