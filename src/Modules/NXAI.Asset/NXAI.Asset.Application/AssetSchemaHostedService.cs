using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NXAI.Asset.Repository;
using NXAI.Infra.IdGenerater.Yitter;
using NXAI.Asset.Repository.Entities;

namespace NXAI.Asset.Application;

/// <summary>启动时确保 ast_ 表存在。与 Member 共用同一库时 EnsureCreated 不够，需补 CreateTables。</summary>
public sealed class AssetSchemaHostedService(IServiceScopeFactory scopes) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = scopes.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AssetDbContext>();

        if (await db.Database.EnsureCreatedAsync(cancellationToken))
        {
            await SeedMainWarehouseAsync(db, cancellationToken);
            return;
        }

        // 库已存在（多半是 Member 先建的）：只补本模块缺的表
        if (!await TableExistsAsync(db, "ast_device_model", cancellationToken))
        {
            var creator = db.GetService<IRelationalDatabaseCreator>();
            await creator.CreateTablesAsync(cancellationToken);
        }

        await SeedMainWarehouseAsync(db, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private static async Task<bool> TableExistsAsync(AssetDbContext db, string tableName, CancellationToken ct)
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

    private static async Task SeedMainWarehouseAsync(AssetDbContext db, CancellationToken ct)
    {
        if (await db.Set<Warehouse>().AnyAsync(ct))
        {
            return;
        }

        if (IdGenerater.CurrentWorkerId < 0)
        {
            try { IdGenerater.SetWorkerId(1); } catch (InvalidOperationException) { }
        }

        db.Set<Warehouse>().Add(new Warehouse
        {
            Id = IdGenerater.GetNextId(),
            Code = "MAIN",
            Name = "主仓"
        });
        await db.SaveChangesAsync(ct);
    }
}
