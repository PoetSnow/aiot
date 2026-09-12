using Microsoft.EntityFrameworkCore;

namespace NXAI.Infra.Repository.EfCore;

public class NullEntityInfo : IEntityInfo
{
    public void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}
