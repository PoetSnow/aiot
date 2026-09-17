using NXAI.Device.Application.Contracts.Dtos;
using NXAI.Device.Application.Contracts.Interfaces;

namespace NXAI.Device.Application.Stores;

/// <summary>第 13 步前的空实现。Append 丢弃，Query 返回空，不挡 ACK。</summary>
public sealed class NullTelemetryStore : ITelemetryStore
{
    public Task AppendAsync(IReadOnlyList<TelemetryPointDto> points, CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    public Task<IReadOnlyList<TelemetryPointDto>> QueryAsync(
        string sn,
        string metric,
        DateTime from,
        DateTime to,
        long? taskId,
        int limit,
        CancellationToken cancellationToken = default)
        => Task.FromResult<IReadOnlyList<TelemetryPointDto>>([]);
}
