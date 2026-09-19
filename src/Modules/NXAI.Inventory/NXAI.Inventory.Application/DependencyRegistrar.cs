using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NXAI.Infra.IdGenerater.Yitter;
using NXAI.Inventory.Repository;
using NXAI.Shared;
using NXAI.Shared.Application.Registrar;

namespace NXAI.Inventory.Application;

/// <summary>耗材模块 DI。归属查询独立注册，不覆盖 Device 接口。</summary>
public sealed class DependencyRegistrar(IServiceCollection services, IServiceInfo serviceInfo, IConfiguration configuration, ServiceLifetime lifetime = ServiceLifetime.Scoped)
    : AbstractApplicationDependencyRegistrar(services, serviceInfo, configuration, lifetime)
{
    protected override Assembly ApplicationLayerAssembly => Assembly.GetExecutingAssembly();

    protected override Assembly RepositoryOrDomainLayerAssembly => typeof(EntityInfo).Assembly;

    public override void AddApplicationServices()
    {
        EnsureWorkerId();
        AddOperater(services);
        AddModuleMySqlDbContext<InventoryDbContext, EntityInfo>(registerDefaultUnitOfWork: true);
        services.AddScoped<Contracts.Interfaces.IInventoryService, Services.InventoryService>();
        services.AddScoped<Contracts.Interfaces.IConsumableOwnershipService, Services.ConsumableOwnershipService>();
        services.AddScoped<Acl.IDeviceSlotGateway, Acl.DeviceSlotGateway>();
        AddCapEventBus([typeof(Subscribers.CookingCompletedSubscriber)]);
        services.AddHostedService<InventorySchemaHostedService>();
    }

    private static void EnsureWorkerId()
    {
        if (IdGenerater.CurrentWorkerId >= 0)
        {
            return;
        }

        try
        {
            IdGenerater.SetWorkerId(1);
        }
        catch (InvalidOperationException)
        {
        }
    }
}
