using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NXAI.Recipe.Repository.Entities;

/// <summary>rcp_material 唯一编码。</summary>
public class MaterialConfig : IEntityTypeConfiguration<Material>
{
    public void Configure(EntityTypeBuilder<Material> builder)
    {
        builder.Property(x => x.Code).HasMaxLength(Material.Code_MaxLength).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(Material.Name_MaxLength).IsRequired();
        builder.Property(x => x.DefaultMode).HasMaxLength(Material.DefaultMode_MaxLength).IsRequired();
        builder.HasIndex(x => x.Code).IsUnique();
    }
}
