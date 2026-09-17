using DotNetCore.CAP;
using NXAI.Cooking.Application.Contracts.Events;
using NXAI.Inventory.Application.Contracts.Interfaces;

namespace NXAI.Inventory.Application.Subscribers;

/// <summary>任务完成后扣 1，缺料失败不扣。</summary>
public sealed class CookingCompletedSubscriber(IInventoryService inventory) : ICapSubscribe
{
    /// <summary>消费制作完成事件并扣账本。</summary>
    /// <param name="payload">完成事件。</param>
    [CapSubscribe(CookingTaskCompletedEvent.Topic)]
    public Task HandleAsync(CookingTaskCompletedEvent payload)
        => inventory.DecrementAsync(payload.ConsumableIds);
}
