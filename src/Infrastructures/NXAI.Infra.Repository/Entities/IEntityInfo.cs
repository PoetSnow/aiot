using Microsoft.EntityFrameworkCore;

namespace NXAI.Infra.Repository;

public interface IEntityInfo
{
    void OnModelCreating(ModelBuilder modelBuilder);
}
