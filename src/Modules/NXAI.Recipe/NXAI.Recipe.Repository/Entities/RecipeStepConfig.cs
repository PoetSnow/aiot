using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NXAI.Recipe.Repository.Entities;

/// <summary>rcp_recipe_step 列长。</summary>
public class RecipeStepConfig : IEntityTypeConfiguration<RecipeStep>
{
    public void Configure(EntityTypeBuilder<RecipeStep> builder)
    {
        builder.Property(x => x.Action).HasMaxLength(RecipeStep.Action_MaxLength).IsRequired();
        builder.Property(x => x.TriggerType).HasMaxLength(RecipeStep.TriggerType_MaxLength).IsRequired();
        builder.Property(x => x.TargetKind).HasMaxLength(RecipeStep.TargetKind_MaxLength);
        builder.Property(x => x.TargetCode).HasMaxLength(RecipeStep.TargetCode_MaxLength);
        builder.Property(x => x.Mode).HasMaxLength(RecipeStep.Mode_MaxLength);
        builder.Property(x => x.AmountUnit).HasMaxLength(RecipeStep.AmountUnit_MaxLength);
        builder.HasIndex(x => new { x.RecipeId, x.StepNo }).IsUnique();
    }
}
