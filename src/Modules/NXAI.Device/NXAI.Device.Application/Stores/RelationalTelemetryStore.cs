using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NXAI.Device.Application.Contracts.Dtos;
using NXAI.Device.Application.Contracts.Interfaces;
using NXAI.Device.Repository;
using NXAI.Device.Repository.Entities;
using NXAI.Infra.IdGenerater.Yitter;

namespace NXAI.Device.Application.Stores;

/// <summary>MySQL 遥测。失败只打日志，不回滚指令。</summary>
public sealed class RelationalTelemetryStore(IServiceScopeFactory scopes, ILogger<RelationalTelemetryStore> logger) : ITelemetryStore
{
    public async Task AppendAsync(IReadOnlyList<TelemetryPointDto> points, CancellationToken cancellationToken = default)
    {
        if (points.Count == 0)
        {
            return;
        }

        try
        {
            using var scope = scopes.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<DeviceDbContext>();
            foreach (var point in points)
            {
                db.Set<DeviceTelemetry>().Add(new DeviceTelemetry
                {
                    Id = IdGenerater.GetNextId(),
                    Sn = point.Sn,
                    Metric = point.Metric,
                    Ts = point.Ts,
                    ReceivedAt = DateTime.Now,
                    ValueNum = point.ValueNum,
                    ValueText = point.ValueText,
                    TaskId = point.TaskId,
                    Epoch = point.Epoch,
                    Quality = "GOOD",
                    Source = "mqtt"
                });
            }

            await db.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "telemetry append failed");
        }
    }

    public async Task<IReadOnlyList<TelemetryPointDto>> QueryAsync(
        string sn,
        string metric,
        DateTime from,
        DateTime to,
        long? taskId,
        int limit,
        CancellationToken cancellationToken = default)
    {
        using var scope = scopes.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DeviceDbContext>();
        var take = limit <= 0 ? 200 : Math.Min(limit, 1000);
        var query = db.Set<DeviceTelemetry>().AsNoTracking()
            .Where(x => x.Sn == sn && x.Metric == metric && x.Ts >= from && x.Ts <= to);
        if (taskId is > 0)
        {
            query = query.Where(x => x.TaskId == taskId);
        }

        var list = await query.OrderBy(x => x.Ts).Take(take).ToListAsync(cancellationToken);
        return list.Select(x => new TelemetryPointDto(x.Sn, x.Metric, x.Ts, x.ValueNum, x.TaskId, x.Epoch, x.ValueText)).ToList();
    }
}
