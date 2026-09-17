using System.Reflection;
using Microsoft.EntityFrameworkCore;
using NXAI.Infra.Repository.EfCore;

namespace NXAI.Cooking.Repository;

/// <summary>烹饪模块表映射。只登记 ckg_ 前缀。</summary>
public class EntityInfo : AbstractEntityInfo
{
    protected override List<Assembly> GetEntityAssemblies() => [GetType().Assembly];

    protected override void SetTableName(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Entities.CookingTask>().ToTable("ckg_task");
        modelBuilder.Entity<Entities.CookingTaskStep>().ToTable("ckg_task_step");
        modelBuilder.Entity<Entities.CookingCommand>().ToTable("ckg_command");
        modelBuilder.Entity<Entities.CookingExecutionLog>().ToTable("ckg_execution_log");
    }
}
