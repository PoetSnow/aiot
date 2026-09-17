using NXAI.Shared.Application.Contracts.Dtos;

namespace NXAI.Device.Application.Contracts.Dtos;

/// <summary>上行 ACK。</summary>
public class DeviceAckDto : InputDto
{
    /// <summary>指令 Id。</summary>
    public long CommandId { get; set; }

    /// <summary>任务 Id。</summary>
    public long? TaskId { get; set; }

    /// <summary>世代。</summary>
    public int? Epoch { get; set; }

    /// <summary>步骤号。</summary>
    public int? StepNo { get; set; }

    /// <summary>结果字面量。</summary>
    public string Result { get; set; } = string.Empty;

    /// <summary>设备水温，仅日志。</summary>
    public decimal? WaterTemp { get; set; }
}
