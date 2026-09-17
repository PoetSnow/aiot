using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NXAI.Catalog.Repository.Entities;

/// <summary>cat_product 唯一 SKU。</summary>
public class ProductConfig : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(x => x.SkuCode).HasMaxLength(Product.SkuCode_MaxLength).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(Product.Name_MaxLength).IsRequired();
        builder.Property(x => x.Form).HasMaxLength(Product.Form_MaxLength).IsRequired();
        builder.Property(x => x.ConsumableTypeCode).HasMaxLength(Product.ConsumableTypeCode_MaxLength).IsRequired();
        builder.Property(x => x.SuggestedRecipeCode).HasMaxLength(Product.SuggestedRecipeCode_MaxLength);
        builder.Property(x => x.CompatibleModels).HasMaxLength(Product.CompatibleModels_MaxLength).IsRequired();
        builder.Property(x => x.QtyUnit).HasMaxLength(Product.QtyUnit_MaxLength).IsRequired();
        builder.Property(x => x.DetailJson).HasMaxLength(Product.DetailJson_MaxLength).IsRequired();
        builder.HasIndex(x => x.SkuCode).IsUnique();
    }
}
