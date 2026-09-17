using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NXAI.Device.Repository.Entities;

/// <summary>dev_capability 一对一设备。</summary>
public class DeviceCapabilityConfig : IEntityTypeConfiguration<DeviceCapability>
{
    public void Configure(EntityTypeBuilder<DeviceCapability> builder)
    {
        builder.Property(x => x.PayloadJson).HasMaxLength(DeviceCapability.PayloadJson_MaxLength).IsRequired();
        builder.HasIndex(x => x.DeviceId).IsUnique();
    }
}
