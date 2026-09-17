using System.Reflection;
using Microsoft.EntityFrameworkCore;
using NXAI.Infra.Repository.EfCore;

namespace NXAI.Member.Repository;

/// <summary>会员模块表映射。只登记 mb_ 前缀表。</summary>
public class EntityInfo : AbstractEntityInfo
{
    protected override List<Assembly> GetEntityAssemblies() => [GetType().Assembly];

    protected override void SetTableName(Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Entities.Member>().ToTable("mb_member");
    }
}
