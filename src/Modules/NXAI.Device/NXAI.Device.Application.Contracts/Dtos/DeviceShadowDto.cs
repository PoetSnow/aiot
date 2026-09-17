using NXAI.Shared.Application.Contracts.Dtos;

namespace NXAI.Device.Application.Contracts.Dtos;

/// <summary>设备影子。只展示，不当投料扳机。</summary>
/// <param name="DeviceId">设备 Id。</param>
/// <param name="Version">影子版本。</param>
/// <param name="Online">是否在线。</param>
/// <param name="WaterTemp">当前水温。</param>
/// <param name="WorkState">工作状态。</param>
/// <param name="CurrentTaskId">设备当前任务。</param>
/// <param name="CurrentEpoch">设备当前世代。</param>
/// <param name="CurrentStepNo">设备当前步骤。</param>
public record DeviceShadowDto(
    long DeviceId,
    int Version,
    bool Online,
    decimal? WaterTemp,
    string WorkState,
    long? CurrentTaskId,
    int? CurrentEpoch,
    int? CurrentStepNo) : IDto;
