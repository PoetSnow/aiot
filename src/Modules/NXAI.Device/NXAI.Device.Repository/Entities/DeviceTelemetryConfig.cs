using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NXAI.Device.Repository.Entities;

/// <summary>dev_telemetry 唯一 (sn, metric, ts)。</summary>
public class DeviceTelemetryConfig : IEntityTypeConfiguration<DeviceTelemetry>
{
    public void Configure(EntityTypeBuilder<DeviceTelemetry> builder)
    {
        builder.Property(x => x.Sn).HasMaxLength(DeviceTelemetry.Sn_MaxLength).IsRequired();
        builder.Property(x => x.Metric).HasMaxLength(DeviceTelemetry.Metric_MaxLength).IsRequired();
        builder.Property(x => x.ValueText).HasMaxLength(DeviceTelemetry.ValueText_MaxLength);
        builder.Property(x => x.Quality).HasMaxLength(DeviceTelemetry.Quality_MaxLength).IsRequired();
        builder.Property(x => x.Source).HasMaxLength(DeviceTelemetry.Source_MaxLength).IsRequired();
        builder.HasIndex(x => new { x.Sn, x.Metric, x.Ts }).IsUnique();
    }
}
