using System.Reflection;
using NXAI.Infra.Repository.EfCore;

namespace NXAI.Device.Repository;

public class EntityInfo : AbstractEntityInfo
{
    protected override List<Assembly> GetEntityAssemblies() => [GetType().Assembly];

    protected override void SetTableName(Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)
    {
        // Table prefix dev_ . Entities are added in a later step.
    }
}
