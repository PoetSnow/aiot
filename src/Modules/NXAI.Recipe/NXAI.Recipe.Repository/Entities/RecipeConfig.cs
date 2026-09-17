using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NXAI.Recipe.Repository.Entities;

/// <summary>rcp_recipe 唯一 (Code, Version)。</summary>
public class RecipeConfig : IEntityTypeConfiguration<Recipe>
{
    public void Configure(EntityTypeBuilder<Recipe> builder)
    {
        builder.Property(x => x.Code).HasMaxLength(Recipe.Code_MaxLength).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(Recipe.Name_MaxLength).IsRequired();
        builder.Property(x => x.SceneTags).HasMaxLength(Recipe.SceneTags_MaxLength).IsRequired();
        builder.Property(x => x.CompatibleModels).HasMaxLength(Recipe.CompatibleModels_MaxLength).IsRequired();
        builder.HasIndex(x => new { x.Code, x.Version }).IsUnique();
    }
}
