using NXAI.Infra.Repository;

namespace NXAI.Cooking.Repository.Entities;

/// <summary>执行日志，表 ckg_execution_log。</summary>
public class CookingExecutionLog : EfEntity
{
    public const int Message_MaxLength = 256;

    /// <summary>任务 Id。</summary>
    public long TaskId { get; set; }

    /// <summary>指令 Id。</summary>
    public long? CommandId { get; set; }

    /// <summary>说明。</summary>
    public string Message { get; set; } = string.Empty;
}
