using System.Reflection;
using Microsoft.EntityFrameworkCore;
using NXAI.Infra.Repository.EfCore;

namespace NXAI.Asset.Repository;

/// <summary>资产模块表映射。只登记 ast_ 前缀表。</summary>
public class EntityInfo : AbstractEntityInfo
{
    protected override List<Assembly> GetEntityAssemblies() => [GetType().Assembly];

    protected override void SetTableName(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Entities.DeviceModel>().ToTable("ast_device_model");
        modelBuilder.Entity<Entities.Warehouse>().ToTable("ast_warehouse");
        modelBuilder.Entity<Entities.AssetSn>().ToTable("ast_sn");
        modelBuilder.Entity<Entities.StockIn>().ToTable("ast_stock_in");
        modelBuilder.Entity<Entities.StockInLine>().ToTable("ast_stock_in_line");
        modelBuilder.Entity<Entities.StockOut>().ToTable("ast_stock_out");
        modelBuilder.Entity<Entities.StockOutLine>().ToTable("ast_stock_out_line");
    }
}
