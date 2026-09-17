using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NXAI.Cooking.Repository.Entities;

/// <summary>ckg_task_step 列长。</summary>
public class CookingTaskStepConfig : IEntityTypeConfiguration<CookingTaskStep>
{
    public void Configure(EntityTypeBuilder<CookingTaskStep> builder)
    {
        builder.Property(x => x.Action).HasMaxLength(CookingTaskStep.Action_MaxLength).IsRequired();
        builder.Property(x => x.SlotCode).HasMaxLength(CookingTaskStep.SlotCode_MaxLength);
        builder.Property(x => x.Mode).HasMaxLength(CookingTaskStep.Mode_MaxLength);
        builder.Property(x => x.AmountUnit).HasMaxLength(CookingTaskStep.AmountUnit_MaxLength);
        builder.Property(x => x.TriggerType).HasMaxLength(CookingTaskStep.TriggerType_MaxLength).IsRequired();
        builder.Property(x => x.Result).HasMaxLength(CookingTaskStep.Result_MaxLength);
        builder.HasIndex(x => new { x.TaskId, x.StepNo }).IsUnique();
    }
}
