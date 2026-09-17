using DotNetCore.CAP;
using NXAI.Cooking.Application.Contracts.Interfaces;
using NXAI.Device.Application.Contracts.Events;

namespace NXAI.Cooking.Application.Subscribers;

/// <summary>只认 ACK 推进状态机，不因影子 80℃ 发立即投放。</summary>
public sealed class DeviceAckSubscriber(ICookingService cooking) : ICapSubscribe
{
    /// <summary>消费设备 ACK，推进当前步骤。</summary>
    /// <param name="payload">ACK 事件。</param>
    [CapSubscribe(DeviceCommandAckedEvent.Topic)]
    public Task HandleAsync(DeviceCommandAckedEvent payload)
        => cooking.HandleAckAsync(payload.CommandId, payload.Result, payload.Epoch);
}
