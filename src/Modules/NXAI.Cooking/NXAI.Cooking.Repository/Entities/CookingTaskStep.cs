using NXAI.Infra.Repository;

namespace NXAI.Cooking.Repository.Entities;

/// <summary>任务步骤，表 ckg_task_step。已解析到仓位。</summary>
public class CookingTaskStep : EfEntity
{
    public const int Action_MaxLength = 16;
    public const int SlotCode_MaxLength = 16;
    public const int Mode_MaxLength = 16;
    public const int AmountUnit_MaxLength = 8;
    public const int TriggerType_MaxLength = 16;
    public const int Result_MaxLength = 16;

    /// <summary>任务 Id。</summary>
    public long TaskId { get; set; }

    /// <summary>步骤序号。</summary>
    public int StepNo { get; set; }

    /// <summary>动作。</summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>解析后的仓位。HEAT 可空。</summary>
    public string? SlotCode { get; set; }

    /// <summary>投料模式。</summary>
    public string? Mode { get; set; }

    /// <summary>数量。</summary>
    public decimal? AmountValue { get; set; }

    /// <summary>单位。</summary>
    public string? AmountUnit { get; set; }

    /// <summary>触发。</summary>
    public string TriggerType { get; set; } = "IMMEDIATE";

    /// <summary>目标温度。</summary>
    public decimal? TempCelsius { get; set; }

    /// <summary>当前指令 Id。</summary>
    public long? CommandId { get; set; }

    /// <summary>耗材实例。</summary>
    public long? ConsumableId { get; set; }

    /// <summary>步骤结果。</summary>
    public string? Result { get; set; }
}
