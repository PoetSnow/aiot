using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NXAI.Device.Repository;

namespace NXAI.Device.Application;

/// <summary>启动时确保 dev_ 表存在。库已被 Member/Asset 建过时走 CreateTables。</summary>
public sealed class DeviceSchemaHostedService(IServiceScopeFactory scopes) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = scopes.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DeviceDbContext>();

        if (await db.Database.EnsureCreatedAsync(cancellationToken))
        {
            await EnsureTelemetryTableAsync(db, cancellationToken);
            return;
        }

        if (!await TableExistsAsync(db, "dev_device", cancellationToken))
        {
            var creator = db.GetService<IRelationalDatabaseCreator>();
            await creator.CreateTablesAsync(cancellationToken);
        }

        await EnsureTelemetryTableAsync(db, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private static async Task<bool> TableExistsAsync(DeviceDbContext db, string tableName, CancellationToken ct)
    {
        await db.Database.OpenConnectionAsync(ct);
        try
        {
            await using var cmd = db.Database.GetDbConnection().CreateCommand();
            cmd.CommandText =
                "SELECT 1 FROM information_schema.tables WHERE table_schema = DATABASE() AND table_name = @t LIMIT 1";
            var parameter = cmd.CreateParameter();
            parameter.ParameterName = "@t";
            parameter.Value = tableName;
            cmd.Parameters.Add(parameter);
            var result = await cmd.ExecuteScalarAsync(ct);
            return result is not null && result is not DBNull;
        }
        finally
        {
            await db.Database.CloseConnectionAsync();
        }
    }

    private static async Task EnsureTelemetryTableAsync(DeviceDbContext db, CancellationToken ct)
    {
        if (await TableExistsAsync(db, "dev_telemetry", ct))
        {
            return;
        }

        await db.Database.ExecuteSqlRawAsync(
            """
            CREATE TABLE IF NOT EXISTS dev_telemetry (
              id bigint NOT NULL,
              sn varchar(64) NOT NULL,
              metric varchar(32) NOT NULL,
              ts datetime(6) NOT NULL,
              received_at datetime(6) NOT NULL,
              value_num decimal(18,4) NULL,
              value_text varchar(64) NULL,
              task_id bigint NULL,
              epoch int NULL,
              quality varchar(16) NOT NULL,
              source varchar(16) NOT NULL,
              PRIMARY KEY (id),
              UNIQUE KEY ux_dev_telemetry_sn_metric_ts (sn, metric, ts)
            )
            """,
            ct);
    }
}
