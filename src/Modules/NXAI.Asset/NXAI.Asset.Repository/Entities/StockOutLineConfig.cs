using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NXAI.Asset.Repository.Entities;

/// <summary>ast_stock_out_line 列长。</summary>
public class StockOutLineConfig : IEntityTypeConfiguration<StockOutLine>
{
    public void Configure(EntityTypeBuilder<StockOutLine> builder)
    {
        builder.Property(x => x.Sn).HasMaxLength(StockOutLine.Sn_MaxLength).IsRequired();
        builder.HasIndex(x => new { x.StockOutId, x.Sn }).IsUnique();
    }
}
