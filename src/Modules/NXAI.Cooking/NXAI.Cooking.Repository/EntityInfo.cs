using System.Reflection;
using NXAI.Infra.Repository.EfCore;

namespace NXAI.Cooking.Repository;

public class EntityInfo : AbstractEntityInfo
{
    protected override List<Assembly> GetEntityAssemblies() => [GetType().Assembly];

    protected override void SetTableName(Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)
    {
        // Table prefix ckg_ . Entities are added in a later step.
    }
}
