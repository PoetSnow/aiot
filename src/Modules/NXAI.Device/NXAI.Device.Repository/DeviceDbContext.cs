using Microsoft.EntityFrameworkCore;
using NXAI.Infra.Repository;
using NXAI.Infra.Repository.EfCore.MySql;

namespace NXAI.Device.Repository;

/// <summary>设备上下文，只含 dev_ 表。禁止出现 ast_ / mb_ 实体。</summary>
public class DeviceDbContext(DbContextOptions<DeviceDbContext> options, EntityInfo entityInfo, Operater operater)
    : MySqlDbContext(options, entityInfo, operater);
