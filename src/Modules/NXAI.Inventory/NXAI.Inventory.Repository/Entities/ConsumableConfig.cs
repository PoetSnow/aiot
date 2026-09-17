using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NXAI.Inventory.Repository.Entities;

/// <summary>inv_consumable 列长。</summary>
public class ConsumableConfig : IEntityTypeConfiguration<Consumable>
{
    public void Configure(EntityTypeBuilder<Consumable> builder)
    {
        builder.Property(x => x.ConsumableTypeCode).HasMaxLength(Consumable.ConsumableTypeCode_MaxLength).IsRequired();
        builder.Property(x => x.Form).HasMaxLength(Consumable.Form_MaxLength).IsRequired();
        builder.Property(x => x.QtyUnit).HasMaxLength(Consumable.QtyUnit_MaxLength).IsRequired();
        builder.Property(x => x.PlacedSlotCode).HasMaxLength(Consumable.SlotCode_MaxLength);
        builder.HasIndex(x => x.MemberId);
    }
}
