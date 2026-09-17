using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NXAI.Asset.Repository.Entities;

/// <summary>ast_stock_out 列长。</summary>
public class StockOutConfig : IEntityTypeConfiguration<StockOut>
{
    public void Configure(EntityTypeBuilder<StockOut> builder)
    {
        builder.Property(x => x.BillNo).HasMaxLength(StockOut.BillNo_MaxLength).IsRequired();
        builder.Property(x => x.Remark).HasMaxLength(StockOut.Remark_MaxLength).IsRequired();
        builder.HasIndex(x => x.BillNo).IsUnique();
    }
}
