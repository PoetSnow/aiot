using Microsoft.EntityFrameworkCore;
using NXAI.Infra.Repository;
using NXAI.Infra.Repository.EfCore.MySql;

namespace NXAI.Asset.Repository;

public class AssetDbContext(DbContextOptions<AssetDbContext> options, EntityInfo entityInfo, Operater operater)
    : MySqlDbContext(options, entityInfo, operater);
