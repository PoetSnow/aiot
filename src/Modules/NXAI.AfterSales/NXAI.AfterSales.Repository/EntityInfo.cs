using System.Reflection;
using Microsoft.EntityFrameworkCore;
using NXAI.Infra.Repository.EfCore;

namespace NXAI.AfterSales.Repository;

/// <summary>售后模块表映射。只登记 afs_ 前缀。不直接改 ast_sn。</summary>
public class EntityInfo : AbstractEntityInfo
{
    protected override List<Assembly> GetEntityAssemblies() => [GetType().Assembly];

    protected override void SetTableName(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Entities.Ticket>().ToTable("afs_ticket");
        modelBuilder.Entity<Entities.TicketLog>().ToTable("afs_ticket_log");
    }
}
