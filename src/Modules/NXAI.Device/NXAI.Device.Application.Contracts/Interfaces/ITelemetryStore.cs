using NXAI.Device.Application.Contracts.Dtos;

namespace NXAI.Device.Application.Contracts.Interfaces;

/// <summary>遥测写入口。第 13 步才有 MySQL 实现。失败不得回滚指令 ACK。</summary>
public interface ITelemetryStore
{
    /// <summary>追加遥测点。失败只打日志。</summary>
    Task AppendAsync(IReadOnlyList<TelemetryPointDto> points, CancellationToken cancellationToken = default);

    /// <summary>按 SN + 指标查时间范围。</summary>
    Task<IReadOnlyList<TelemetryPointDto>> QueryAsync(
        string sn,
        string metric,
        DateTime from,
        DateTime to,
        long? taskId,
        int limit,
        CancellationToken cancellationToken = default);
}
