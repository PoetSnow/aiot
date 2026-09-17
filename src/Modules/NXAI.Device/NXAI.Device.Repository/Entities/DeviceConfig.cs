using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NXAI.Device.Repository.Entities;

/// <summary>dev_device 列长与唯一 SN。</summary>
public class DeviceConfig : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.Property(x => x.DeviceSn).HasMaxLength(Device.DeviceSn_MaxLength).IsRequired();
        builder.Property(x => x.ModelCode).HasMaxLength(Device.ModelCode_MaxLength).IsRequired();
        builder.Property(x => x.DeviceTokenHash).HasMaxLength(Device.TokenHash_MaxLength).IsRequired();
        builder.HasIndex(x => x.DeviceSn).IsUnique();
        builder.HasIndex(x => x.MemberId);
    }
}
