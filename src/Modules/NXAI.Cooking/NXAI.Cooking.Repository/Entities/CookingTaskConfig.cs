using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NXAI.Cooking.Repository.Entities;

/// <summary>ckg_task 列长。</summary>
public class CookingTaskConfig : IEntityTypeConfiguration<CookingTask>
{
    public void Configure(EntityTypeBuilder<CookingTask> builder)
    {
        builder.Property(x => x.RecipeCode).HasMaxLength(CookingTask.RecipeCode_MaxLength).IsRequired();
        builder.Property(x => x.SnapshotJson).HasMaxLength(CookingTask.SnapshotJson_MaxLength).IsRequired();
        builder.Property(x => x.RejectReason).HasMaxLength(CookingTask.RejectReason_MaxLength);
        builder.HasIndex(x => x.DeviceId);
        builder.HasIndex(x => x.MemberId);
    }
}
