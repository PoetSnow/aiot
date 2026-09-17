using NXAI.Infra.Repository;

namespace NXAI.Device.Repository.Entities;

/// <summary>遥测点，表 dev_telemetry。只由 ITelemetryStore 写入。</summary>
public class DeviceTelemetry : EfEntity
{
    public const int Sn_MaxLength = 64;
    public const int Metric_MaxLength = 32;
    public const int ValueText_MaxLength = 64;
    public const int Quality_MaxLength = 16;
    public const int Source_MaxLength = 16;

    /// <summary>设备 SN。</summary>
    public string Sn { get; set; } = string.Empty;

    /// <summary>指标名，如 water_temp。</summary>
    public string Metric { get; set; } = string.Empty;

    /// <summary>设备事件时间。</summary>
    public DateTime Ts { get; set; }

    /// <summary>服务器接收时间。</summary>
    public DateTime ReceivedAt { get; set; }

    /// <summary>数值。</summary>
    public decimal? ValueNum { get; set; }

    /// <summary>文本值，如 IDLE。</summary>
    public string? ValueText { get; set; }

    /// <summary>关联任务。</summary>
    public long? TaskId { get; set; }

    /// <summary>世代。</summary>
    public int? Epoch { get; set; }

    /// <summary>质量。</summary>
    public string Quality { get; set; } = "GOOD";

    /// <summary>来源，如 mqtt。</summary>
    public string Source { get; set; } = "mqtt";
}
