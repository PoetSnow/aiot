using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NXAI.Device.Repository.Entities;

/// <summary>dev_slot 列长。同一设备仓位码唯一。</summary>
public class DeviceSlotConfig : IEntityTypeConfiguration<DeviceSlot>
{
    public void Configure(EntityTypeBuilder<DeviceSlot> builder)
    {
        builder.Property(x => x.SlotCode).HasMaxLength(DeviceSlot.SlotCode_MaxLength).IsRequired();
        builder.Property(x => x.SlotType).HasMaxLength(DeviceSlot.SlotType_MaxLength).IsRequired();
        builder.Property(x => x.SupportedModes).HasMaxLength(DeviceSlot.SupportedModes_MaxLength).IsRequired();
        builder.Property(x => x.MaterialCode).HasMaxLength(DeviceSlot.MaterialCode_MaxLength);
        builder.HasIndex(x => new { x.DeviceId, x.SlotCode }).IsUnique();
    }
}
