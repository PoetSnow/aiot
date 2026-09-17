using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NXAI.Device.Repository.Entities;

/// <summary>dev_shadow 一对一设备。</summary>
public class DeviceShadowConfig : IEntityTypeConfiguration<DeviceShadow>
{
    public void Configure(EntityTypeBuilder<DeviceShadow> builder)
    {
        builder.Property(x => x.WorkState).HasMaxLength(DeviceShadow.WorkState_MaxLength).IsRequired();
        builder.Property(x => x.SlotsOccupiedJson).HasMaxLength(DeviceShadow.SlotsOccupiedJson_MaxLength).IsRequired();
        builder.HasIndex(x => x.DeviceId).IsUnique();
    }
}
