using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NXAI.Asset.Repository.Entities;

/// <summary>ast_device_model 列长与唯一索引。</summary>
public class DeviceModelConfig : IEntityTypeConfiguration<DeviceModel>
{
    public void Configure(EntityTypeBuilder<DeviceModel> builder)
    {
        builder.Property(x => x.ModelCode).HasMaxLength(DeviceModel.ModelCode_MaxLength).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(DeviceModel.Name_MaxLength).IsRequired();
        builder.Property(x => x.SlotProfileJson).HasMaxLength(DeviceModel.SlotProfileJson_MaxLength).IsRequired();
        builder.HasIndex(x => x.ModelCode).IsUnique();
    }
}
