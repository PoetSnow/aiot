namespace NXAI.Device.Application.Contracts.Events;

/// <summary>设备 ACK 到达。Cooking 只据此推进，不看影子水温开火。</summary>
public sealed class DeviceCommandAckedEvent
{
    /// <summary>主题。</summary>
    public const string Topic = "pot.device.command.acked.v1";

    /// <summary>指令 Id。</summary>
    public long CommandId { get; set; }

    /// <summary>设备 Id。</summary>
    public long DeviceId { get; set; }

    /// <summary>任务 Id。</summary>
    public long? TaskId { get; set; }

    /// <summary>世代。</summary>
    public int? Epoch { get; set; }

    /// <summary>步骤号。</summary>
    public int? StepNo { get; set; }

    /// <summary>ACK 结果字面量，如 SUCCESS / REJECTED。</summary>
    public string Result { get; set; } = string.Empty;

    /// <summary>设备上报水温，仅日志。</summary>
    public decimal? WaterTemp { get; set; }
}
