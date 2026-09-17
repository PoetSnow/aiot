using NXAI.Shared.Application.Contracts.Dtos;

namespace NXAI.Recipe.Application.Contracts.Dtos;

/// <summary>配方步骤。</summary>
public class RecipeStepDto : OutputDto
{
    /// <summary>步骤序号。</summary>
    public int StepNo { get; set; }

    /// <summary>动作。</summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>触发类型。</summary>
    public string TriggerType { get; set; } = string.Empty;

    /// <summary>目标温度。</summary>
    public decimal? TempCelsius { get; set; }

    /// <summary>延迟秒数。</summary>
    public int? DelaySeconds { get; set; }

    /// <summary>目标种类。</summary>
    public string? TargetKind { get; set; }

    /// <summary>目标编码，不是仓位号。</summary>
    public string? TargetCode { get; set; }

    /// <summary>投料模式。</summary>
    public string? Mode { get; set; }

    /// <summary>数量。</summary>
    public decimal? AmountValue { get; set; }

    /// <summary>单位。</summary>
    public string? AmountUnit { get; set; }
}
