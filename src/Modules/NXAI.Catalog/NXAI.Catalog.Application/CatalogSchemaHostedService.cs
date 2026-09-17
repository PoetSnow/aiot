using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NXAI.Catalog.Repository;
using NXAI.Catalog.Repository.Entities;
using NXAI.Infra.IdGenerater.Yitter;

namespace NXAI.Catalog.Application;

/// <summary>启动时确保 cat_ 表存在，并写入茶包货架示例。</summary>
public sealed class CatalogSchemaHostedService(IServiceScopeFactory scopes) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = scopes.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();

        if (!await db.Database.EnsureCreatedAsync(cancellationToken)
            && !await TableExistsAsync(db, "cat_product", cancellationToken))
        {
            var creator = db.GetService<IRelationalDatabaseCreator>();
            await creator.CreateTablesAsync(cancellationToken);
        }

        await SeedAsync(db, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private static async Task<bool> TableExistsAsync(CatalogDbContext db, string tableName, CancellationToken ct)
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

    private static async Task SeedAsync(CatalogDbContext db, CancellationToken ct)
    {
        if (await db.Set<Product>().AnyAsync(ct))
        {
            return;
        }

        if (IdGenerater.CurrentWorkerId < 0)
        {
            try { IdGenerater.SetWorkerId(1); } catch (InvalidOperationException) { }
        }

        db.Set<Product>().Add(new Product
        {
            Id = IdGenerater.GetNextId(),
            SkuCode = "TEA_PACK",
            Name = "茶包",
            Form = "PACKAGE",
            ConsumableTypeCode = "TEA_PACK",
            SuggestedRecipeCode = "HOT_TEA",
            CompatibleModels = string.Empty,
            PackageQty = 1,
            QtyUnit = "PACK",
            Status = ProductStatus.OnShelf,
            DetailJson = "{}"
        });
        await db.SaveChangesAsync(ct);
    }
}
