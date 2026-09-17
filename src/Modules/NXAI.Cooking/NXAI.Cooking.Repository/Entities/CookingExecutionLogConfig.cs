using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NXAI.Cooking.Repository.Entities;

/// <summary>ckg_execution_log 列长。</summary>
public class CookingExecutionLogConfig : IEntityTypeConfiguration<CookingExecutionLog>
{
    public void Configure(EntityTypeBuilder<CookingExecutionLog> builder)
    {
        builder.Property(x => x.Message).HasMaxLength(CookingExecutionLog.Message_MaxLength).IsRequired();
        builder.HasIndex(x => x.TaskId);
    }
}
