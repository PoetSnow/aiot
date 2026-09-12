using Microsoft.Extensions.DependencyInjection;
using NXAI.Infra.Core.DependencyInjection;
using NXAI.Infra.EventBus;
using NXAI.Infra.Repository;

namespace NXAI.Shared.Domain.Entities;

public abstract class AggregateRoot : DomainEntity, IConcurrency, IEfEntity<long>
{
    public Lazy<IEventPublisher> EventPublisher => new(() =>
    {
        return ServiceLocator.GetProvider().GetRequiredService<IEventPublisher>();
    });

    public byte[] RowVersion { get; set; } = [];
}
