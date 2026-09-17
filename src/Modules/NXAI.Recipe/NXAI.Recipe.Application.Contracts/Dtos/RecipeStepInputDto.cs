using NXAI.Shared.Application.Contracts.Dtos;

namespace NXAI.Recipe.Application.Contracts.Dtos;

/// <summary>配方步骤入参。禁止出现仓位号。</summary>
public class RecipeStepInputDto : InputDto
{
    /// <summary>步骤序号，从 1 起。</summary>
    public int StepNo { get; set; }

    /// <summary>动作：HEAT / DISPENSE / KEEP_WARM / STOP。</summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>触发：TEMP_GTE / DELAY / IMMEDIATE。</summary>
    public string TriggerType { get; set; } = "IMMEDIATE";

    /// <summary>TEMP_GTE 摄氏度。</summary>
    public decimal? TempCelsius { get; set; }

    /// <summary>DELAY 秒数。</summary>
    public int? DelaySeconds { get; set; }

    /// <summary>目标种类，投料为 MATERIAL。</summary>
    public string? TargetKind { get; set; }

    /// <summary>目标编码，如 TEA_PACK。Cooking 再解析到仓位。</summary>
    public string? TargetCode { get; set; }

    /// <summary>投料模式。</summary>
    public string? Mode { get; set; }

    /// <summary>数量。</summary>
    public decimal? AmountValue { get; set; }

    /// <summary>单位 PACK / G / PCS。</summary>
    public string? AmountUnit { get; set; }
}
