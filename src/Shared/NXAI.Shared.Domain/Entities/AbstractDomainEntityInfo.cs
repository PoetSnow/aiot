using NXAI.Infra.Repository;
using NXAI.Infra.Repository.EfCore;
using NXAI.Shared.Domain.Entities;
using NXAI.Shared.Repository.EfCoreEntities;
using System.Reflection;

namespace nxai.Shared.Domain.Entities;

public abstract class AbstractDomainEntityInfo : AbstractEntityInfo, IEntityInfo
{
    protected override List<Type> GetEntityTypes(IEnumerable<Assembly> assemblies)
    {
        var typeList = assemblies.SelectMany(assembly => assembly.GetTypes()
                                                  .Where(m => m.FullName != null
                                                   && (typeof(AggregateRoot).IsAssignableFrom(m) || typeof(DomainEntity).IsAssignableFrom(m))
                                                   && !m.IsAbstract)) ?? [];
        return typeList.Append(typeof(EventTracker)).ToList();
    }
}
