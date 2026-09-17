using System.Reflection;
using Microsoft.EntityFrameworkCore;
using NXAI.Infra.Repository.EfCore;

namespace NXAI.Inventory.Repository;

/// <summary>耗材模块表映射。只登记 inv_ 前缀。</summary>
public class EntityInfo : AbstractEntityInfo
{
    protected override List<Assembly> GetEntityAssemblies() => [GetType().Assembly];

    protected override void SetTableName(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Entities.Consumable>().ToTable("inv_consumable");
    }
}
