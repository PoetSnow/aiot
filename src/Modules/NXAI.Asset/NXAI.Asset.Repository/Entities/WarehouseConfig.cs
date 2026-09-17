using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NXAI.Asset.Repository.Entities;

/// <summary>ast_warehouse 列长与唯一索引。</summary>
public class WarehouseConfig : IEntityTypeConfiguration<Warehouse>
{
    public void Configure(EntityTypeBuilder<Warehouse> builder)
    {
        builder.Property(x => x.Code).HasMaxLength(Warehouse.Code_MaxLength).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(Warehouse.Name_MaxLength).IsRequired();
        builder.HasIndex(x => x.Code).IsUnique();
    }
}
