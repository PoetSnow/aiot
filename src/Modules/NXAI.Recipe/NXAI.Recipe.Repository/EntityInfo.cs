using System.Reflection;
using Microsoft.EntityFrameworkCore;
using NXAI.Infra.Repository.EfCore;

namespace NXAI.Recipe.Repository;

/// <summary>配方模块表映射。只登记 rcp_ 前缀。</summary>
public class EntityInfo : AbstractEntityInfo
{
    protected override List<Assembly> GetEntityAssemblies() => [GetType().Assembly];

    protected override void SetTableName(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Entities.Material>().ToTable("rcp_material");
        modelBuilder.Entity<Entities.Recipe>().ToTable("rcp_recipe");
        modelBuilder.Entity<Entities.RecipeStep>().ToTable("rcp_recipe_step");
    }
}
