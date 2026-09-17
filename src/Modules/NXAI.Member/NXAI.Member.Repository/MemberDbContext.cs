using Microsoft.EntityFrameworkCore;
using NXAI.Infra.Repository;
using NXAI.Infra.Repository.EfCore.MySql;

namespace NXAI.Member.Repository;

/// <summary>会员上下文，只含 mb_ 表，禁止出现 sys_user。</summary>
public class MemberDbContext(DbContextOptions<MemberDbContext> options, EntityInfo entityInfo, Operater operater)
    : MySqlDbContext(options, entityInfo, operater);
