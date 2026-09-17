using NXAI.Shared.Application.Contracts.Dtos;

namespace NXAI.Cooking.Application.Contracts.Dtos;

/// <summary>制作任务。</summary>
public class CookingTaskDto : OutputDto
{
    /// <summary>设备 Id。</summary>
    public long DeviceId { get; set; }

    /// <summary>配方编码。</summary>
    public string RecipeCode { get; set; } = string.Empty;

    /// <summary>配方版本。</summary>
    public int RecipeVersion { get; set; }

    /// <summary>状态。</summary>
    public int Status { get; set; }

    /// <summary>当前步骤。</summary>
    public int CurrentStepNo { get; set; }

    /// <summary>世代。</summary>
    public int Epoch { get; set; }

    /// <summary>拒绝原因。</summary>
    public string? RejectReason { get; set; }

    /// <summary>步骤。</summary>
    public List<CookingTaskStepDto> Steps { get; set; } = [];
}

/// <summary>任务步骤。</summary>
public class CookingTaskStepDto : OutputDto
{
    /// <summary>步骤号。</summary>
    public int StepNo { get; set; }

    /// <summary>动作。</summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>仓位。</summary>
    public string? SlotCode { get; set; }

    /// <summary>指令 Id。</summary>
    public long? CommandId { get; set; }

    /// <summary>结果。</summary>
    public string? Result { get; set; }
}
