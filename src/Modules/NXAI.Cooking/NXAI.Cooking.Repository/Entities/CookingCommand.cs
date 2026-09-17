using NXAI.Infra.Repository;

namespace NXAI.Cooking.Repository.Entities;

/// <summary>下发指令，表 ckg_command。Id 即 commandId。</summary>
public class CookingCommand : EfEntity
{
    public const int IdempotencyKey_MaxLength = 64;
    public const int Action_MaxLength = 16;
    public const int Result_MaxLength = 16;
    public const int IssuePolicy_MaxLength = 16;

    /// <summary>任务 Id。</summary>
    public long TaskId { get; set; }

    /// <summary>步骤 Id。</summary>
    public long StepId { get; set; }

    /// <summary>设备 Id。</summary>
    public long DeviceId { get; set; }

    /// <summary>投递序号。</summary>
    public int Seq { get; set; } = 1;

    /// <summary>幂等键 {taskId}:{stepId}:{seq}。</summary>
    public string IdempotencyKey { get; set; } = string.Empty;

    /// <summary>动作。</summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>DISPENSE 禁止换新 Id 重投。</summary>
    public string IssuePolicy { get; set; } = "RETRY_SAME";

    /// <summary>最近结果。</summary>
    public string? Result { get; set; }

    /// <summary>下发时间，看门狗用。</summary>
    public DateTime IssuedAt { get; set; }
}
