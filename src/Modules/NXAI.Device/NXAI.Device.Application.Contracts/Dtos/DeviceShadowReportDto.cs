using NXAI.Shared.Application.Contracts.Dtos;

namespace NXAI.Device.Application.Contracts.Dtos;

/// <summary>设备上报影子。只覆盖当前值，不当投料扳机。</summary>
public class DeviceShadowReportDto : InputDto
{
    /// <summary>影子版本。</summary>
    public int? Version { get; set; }

    /// <summary>是否在线。</summary>
    public bool? Online { get; set; }

    /// <summary>水温。</summary>
    public decimal? WaterTemp { get; set; }

    /// <summary>工作状态。</summary>
    public string? WorkState { get; set; }

    /// <summary>设备当前任务。</summary>
    public long? CurrentTaskId { get; set; }

    /// <summary>设备当前世代。</summary>
    public int? CurrentEpoch { get; set; }

    /// <summary>设备当前步骤。</summary>
    public int? CurrentStepNo { get; set; }
}
