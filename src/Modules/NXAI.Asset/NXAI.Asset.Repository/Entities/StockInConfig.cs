using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NXAI.Asset.Repository.Entities;

/// <summary>ast_stock_in 列长。</summary>
public class StockInConfig : IEntityTypeConfiguration<StockIn>
{
    public void Configure(EntityTypeBuilder<StockIn> builder)
    {
        builder.Property(x => x.BillNo).HasMaxLength(StockIn.BillNo_MaxLength).IsRequired();
        builder.Property(x => x.Remark).HasMaxLength(StockIn.Remark_MaxLength).IsRequired();
        builder.HasIndex(x => x.BillNo).IsUnique();
    }
}
