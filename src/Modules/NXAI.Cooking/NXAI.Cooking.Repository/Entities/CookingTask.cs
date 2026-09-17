using NXAI.Infra.Repository;

namespace NXAI.Cooking.Repository.Entities;

/// <summary>制作任务，表 ckg_task。执行只认快照。</summary>
public class CookingTask : EfEntity
{
    public const int RecipeCode_MaxLength = 32;
    public const int SnapshotJson_MaxLength = 8000;
    public const int RejectReason_MaxLength = 256;

    /// <summary>会员 Id。</summary>
    public long MemberId { get; set; }

    /// <summary>设备 Id。</summary>
    public long DeviceId { get; set; }

    /// <summary>配方行 Id。</summary>
    public long RecipeId { get; set; }

    /// <summary>配方编码。</summary>
    public string RecipeCode { get; set; } = string.Empty;

    /// <summary>配方版本。</summary>
    public int RecipeVersion { get; set; }

    /// <summary>不可变快照 JSON。</summary>
    public string SnapshotJson { get; set; } = "{}";

    /// <summary>状态，见 <see cref="CookingTaskStatus"/>。</summary>
    public int Status { get; set; } = CookingTaskStatus.Created;

    /// <summary>当前步骤号。</summary>
    public int CurrentStepNo { get; set; }

    /// <summary>任务世代。</summary>
    public int Epoch { get; set; }

    /// <summary>校验失败原因。</summary>
    public string? RejectReason { get; set; }
}

/// <summary>任务状态。</summary>
public static class CookingTaskStatus
{
    /// <summary>刚创建。</summary>
    public const int Created = 0;

    /// <summary>校验中。</summary>
    public const int Validating = 1;

    /// <summary>校验通过待跑。</summary>
    public const int Ready = 2;

    /// <summary>执行中。</summary>
    public const int Running = 3;

    /// <summary>焖泡。</summary>
    public const int Steeping = 4;

    /// <summary>完成。</summary>
    public const int Completed = 5;

    /// <summary>校验失败，零投放。</summary>
    public const int Rejected = 6;

    /// <summary>执行失败。</summary>
    public const int Failed = 7;

    /// <summary>已取消。</summary>
    public const int Cancelled = 8;
}
