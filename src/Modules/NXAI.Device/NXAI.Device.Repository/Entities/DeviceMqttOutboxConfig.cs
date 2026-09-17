using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NXAI.Device.Repository.Entities;

/// <summary>dev_mqtt_outbox。commandId 全局唯一，已 RECEIVED 不能当新投放。</summary>
public class DeviceMqttOutboxConfig : IEntityTypeConfiguration<DeviceMqttOutbox>
{
    public void Configure(EntityTypeBuilder<DeviceMqttOutbox> builder)
    {
        builder.Property(x => x.PayloadJson).HasMaxLength(DeviceMqttOutbox.PayloadJson_MaxLength).IsRequired();
        builder.HasIndex(x => x.CommandId).IsUnique();
        builder.HasIndex(x => new { x.DeviceId, x.Status });
    }
}
