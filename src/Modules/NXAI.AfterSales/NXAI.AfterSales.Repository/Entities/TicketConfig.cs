using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NXAI.AfterSales.Repository.Entities;

/// <summary>afs_ticket 列长。</summary>
public class TicketConfig : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.Property(x => x.TicketNo).HasMaxLength(Ticket.TicketNo_MaxLength).IsRequired();
        builder.Property(x => x.Sn).HasMaxLength(Ticket.Sn_MaxLength).IsRequired();
        builder.Property(x => x.Symptom).HasMaxLength(Ticket.Symptom_MaxLength).IsRequired();
        builder.Property(x => x.CloseResult).HasMaxLength(Ticket.CloseResult_MaxLength);
        builder.HasIndex(x => x.TicketNo).IsUnique();
        builder.HasIndex(x => x.Sn);
    }
}
