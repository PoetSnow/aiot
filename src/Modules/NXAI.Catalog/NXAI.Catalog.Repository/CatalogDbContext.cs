using Microsoft.EntityFrameworkCore;
using NXAI.Infra.Repository;
using NXAI.Infra.Repository.EfCore.MySql;

namespace NXAI.Catalog.Repository;

public class CatalogDbContext(DbContextOptions<CatalogDbContext> options, EntityInfo entityInfo, Operater operater)
    : MySqlDbContext(options, entityInfo, operater);
