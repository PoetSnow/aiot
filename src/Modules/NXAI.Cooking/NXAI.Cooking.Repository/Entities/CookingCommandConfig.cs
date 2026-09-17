using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NXAI.Cooking.Repository.Entities;

/// <summary>ckg_command 列长。</summary>
public class CookingCommandConfig : IEntityTypeConfiguration<CookingCommand>
{
    public void Configure(EntityTypeBuilder<CookingCommand> builder)
    {
        builder.Property(x => x.IdempotencyKey).HasMaxLength(CookingCommand.IdempotencyKey_MaxLength).IsRequired();
        builder.Property(x => x.Action).HasMaxLength(CookingCommand.Action_MaxLength).IsRequired();
        builder.Property(x => x.IssuePolicy).HasMaxLength(CookingCommand.IssuePolicy_MaxLength).IsRequired();
        builder.Property(x => x.Result).HasMaxLength(CookingCommand.Result_MaxLength);
        builder.HasIndex(x => x.IdempotencyKey).IsUnique();
    }
}
