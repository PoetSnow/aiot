using System.Reflection;
using NXAI.Infra.Repository.EfCore;

namespace NXAI.Asset.Repository;

public class EntityInfo : AbstractEntityInfo
{
    protected override List<Assembly> GetEntityAssemblies() => [GetType().Assembly];

    protected override void SetTableName(Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)
    {
        // Table prefix ast_ . Entities are added in a later step.
    }
}
