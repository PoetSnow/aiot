using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NXAI.AfterSales.Repository.Entities;

/// <summary>afs_ticket_log 列长。</summary>
public class TicketLogConfig : IEntityTypeConfiguration<TicketLog>
{
    public void Configure(EntityTypeBuilder<TicketLog> builder)
    {
        builder.Property(x => x.Action).HasMaxLength(TicketLog.Action_MaxLength).IsRequired();
        builder.Property(x => x.Remark).HasMaxLength(TicketLog.Remark_MaxLength).IsRequired();
        builder.HasIndex(x => x.TicketId);
    }
}
