using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NXAI.Infra.IdGenerater.Yitter;
using NXAI.Recipe.Repository;
using NXAI.Recipe.Repository.Entities;
using RecipeEntity = NXAI.Recipe.Repository.Entities.Recipe;

namespace NXAI.Recipe.Application;

/// <summary>启动时确保 rcp_ 表存在，并写入「80℃ 投 1 包」示例配方。</summary>
public sealed class RecipeSchemaHostedService(IServiceScopeFactory scopes) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = scopes.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<RecipeDbContext>();

        if (!await db.Database.EnsureCreatedAsync(cancellationToken)
            && !await TableExistsAsync(db, "rcp_material", cancellationToken))
        {
            var creator = db.GetService<IRelationalDatabaseCreator>();
            await creator.CreateTablesAsync(cancellationToken);
        }

        await SeedHotTeaAsync(db, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private static async Task<bool> TableExistsAsync(RecipeDbContext db, string tableName, CancellationToken ct)
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

    private static async Task SeedHotTeaAsync(RecipeDbContext db, CancellationToken ct)
    {
        if (await db.Set<Material>().AnyAsync(ct))
        {
            return;
        }

        EnsureWorkerId();
        var materialId = IdGenerater.GetNextId();
        db.Set<Material>().Add(new Material
        {
            Id = materialId,
            Code = "TEA_PACK",
            Name = "茶包",
            DefaultMode = "PACKAGE"
        });

        var recipeId = IdGenerater.GetNextId();
        db.Set<RecipeEntity>().Add(new RecipeEntity
        {
            Id = recipeId,
            Code = "HOT_TEA",
            Version = 1,
            Name = "热茶",
            Status = RecipeStatus.Published,
            SceneTags = "tea",
            CompatibleModels = string.Empty
        });

        db.Set<RecipeStep>().Add(new RecipeStep
        {
            Id = IdGenerater.GetNextId(),
            RecipeId = recipeId,
            StepNo = 1,
            Action = "HEAT",
            TriggerType = "TEMP_GTE",
            TempCelsius = 80
        });
        db.Set<RecipeStep>().Add(new RecipeStep
        {
            Id = IdGenerater.GetNextId(),
            RecipeId = recipeId,
            StepNo = 2,
            Action = "DISPENSE",
            TriggerType = "TEMP_GTE",
            TempCelsius = 80,
            TargetKind = "MATERIAL",
            TargetCode = "TEA_PACK",
            Mode = "PACKAGE",
            AmountValue = 1,
            AmountUnit = "PACK"
        });

        await db.SaveChangesAsync(ct);
    }

    private static void EnsureWorkerId()
    {
        if (IdGenerater.CurrentWorkerId >= 0)
        {
            return;
        }

        try
        {
            IdGenerater.SetWorkerId(1);
        }
        catch (InvalidOperationException)
        {
        }
    }
}
