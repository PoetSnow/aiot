using Microsoft.EntityFrameworkCore;
using NXAI.Infra.Repository;
using NXAI.Infra.Repository.EfCore.MySql;

namespace NXAI.System.Repository;

/// <summary>
/// System bounded context DbContext (same MySQL database, isolated EF model).
/// </summary>
public class SystemDbContext(DbContextOptions<SystemDbContext> options, EntityInfo entityInfo, Operater operater)
    : MySqlDbContext(options, entityInfo, operater)
{
}
