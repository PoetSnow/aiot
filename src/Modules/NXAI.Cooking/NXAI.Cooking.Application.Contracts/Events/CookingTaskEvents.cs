namespace NXAI.Cooking.Application.Contracts.Events;

/// <summary>任务完成。Inventory 订阅后扣余量。</summary>
public sealed class CookingTaskCompletedEvent
{
    /// <summary>主题。</summary>
    public const string Topic = "pot.cooking.task.completed.v1";

    /// <summary>任务 Id。</summary>
    public long TaskId { get; set; }

    /// <summary>设备 Id。</summary>
    public long DeviceId { get; set; }

    /// <summary>会员 Id。</summary>
    public long MemberId { get; set; }

    /// <summary>实际投放的耗材实例。</summary>
    public List<long> ConsumableIds { get; set; } = [];
}

/// <summary>任务失败。缺料不扣库存。</summary>
public sealed class CookingTaskFailedEvent
{
    /// <summary>主题。</summary>
    public const string Topic = "pot.cooking.task.failed.v1";

    /// <summary>任务 Id。</summary>
    public long TaskId { get; set; }

    /// <summary>失败原因。</summary>
    public string Reason { get; set; } = string.Empty;
}
