using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NXAI.Cooking.Repository;

namespace NXAI.Cooking.Application;

/// <summary>启动时确保 ckg_ 表存在。</summary>
public sealed class CookingSchemaHostedService(IServiceScopeFactory scopes) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = scopes.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CookingDbContext>();

        if (await db.Database.EnsureCreatedAsync(cancellationToken))
        {
            return;
        }

        if (!await TableExistsAsync(db, "ckg_task", cancellationToken))
        {
            var creator = db.GetService<IRelationalDatabaseCreator>();
            await creator.CreateTablesAsync(cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private static async Task<bool> TableExistsAsync(CookingDbContext db, string tableName, CancellationToken ct)
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
}
