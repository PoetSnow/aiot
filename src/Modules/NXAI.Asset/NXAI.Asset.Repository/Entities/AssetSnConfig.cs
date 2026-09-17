using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NXAI.Asset.Repository.Entities;

/// <summary>ast_sn 列长、唯一 SN、库存查询索引。</summary>
public class AssetSnConfig : IEntityTypeConfiguration<AssetSn>
{
    public void Configure(EntityTypeBuilder<AssetSn> builder)
    {
        builder.Property(x => x.Sn).HasMaxLength(AssetSn.Sn_MaxLength).IsRequired();
        builder.Property(x => x.ModelCode).HasMaxLength(AssetSn.ModelCode_MaxLength).IsRequired();
        builder.HasIndex(x => x.Sn).IsUnique();
        // 库存汇总：按仓+型号筛 InStock
        builder.HasIndex(x => new { x.WarehouseId, x.ModelCode, x.Status });
    }
}
