using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NXAI.Asset.Repository.Entities;

/// <summary>ast_stock_in_line 列长。</summary>
public class StockInLineConfig : IEntityTypeConfiguration<StockInLine>
{
    public void Configure(EntityTypeBuilder<StockInLine> builder)
    {
        builder.Property(x => x.Sn).HasMaxLength(StockInLine.Sn_MaxLength).IsRequired();
        builder.Property(x => x.ModelCode).HasMaxLength(StockInLine.ModelCode_MaxLength).IsRequired();
        builder.HasIndex(x => new { x.StockInId, x.Sn }).IsUnique();
    }
}
