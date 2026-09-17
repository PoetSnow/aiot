using Microsoft.EntityFrameworkCore;
using NXAI.Infra.Repository;
using NXAI.Infra.Repository.EfCore.MySql;

namespace NXAI.Asset.Repository;

/// <summary>资产上下文，只含 ast_ 表。禁止出现 mb_ / sys_ / dev_ 实体。</summary>
public class AssetDbContext(DbContextOptions<AssetDbContext> options, EntityInfo entityInfo, Operater operater)
    : MySqlDbContext(options, entityInfo, operater);
