namespace NXAI.Device.Application.Contracts.Dtos;

/// <summary>遥测点。第 13 步才落 dev_telemetry，此处只留模型。</summary>
/// <param name="Sn">设备 SN。</param>
/// <param name="Metric">指标名，如 water_temp。</param>
/// <param name="Ts">设备事件时间。</param>
/// <param name="ValueNum">数值。</param>
/// <param name="TaskId">关联任务。</param>
/// <param name="Epoch">任务世代。</param>
/// <param name="ValueText">文本值，如 IDLE / true。</param>
public record TelemetryPointDto(
    string Sn,
    string Metric,
    DateTime Ts,
    decimal? ValueNum,
    long? TaskId,
    int? Epoch,
    string? ValueText = null);
