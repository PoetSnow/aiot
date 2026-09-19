using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NXAI.Cooking.Repository;
using NXAI.Infra.IdGenerater.Yitter;
using NXAI.Shared;
using NXAI.Shared.Application.Registrar;

namespace NXAI.Cooking.Application;

/// <summary>烹饪模块 DI。不引用 MQTT 客户端。</summary>
public sealed class DependencyRegistrar(IServiceCollection services, IServiceInfo serviceInfo, IConfiguration configuration, ServiceLifetime lifetime = ServiceLifetime.Scoped)
    : AbstractApplicationDependencyRegistrar(services, serviceInfo, configuration, lifetime)
{
    protected override Assembly ApplicationLayerAssembly => Assembly.GetExecutingAssembly();

    protected override Assembly RepositoryOrDomainLayerAssembly => typeof(EntityInfo).Assembly;

    public override void AddApplicationServices()
    {
        EnsureWorkerId();
        AddOperater(services);
        AddModuleMySqlDbContext<CookingDbContext, EntityInfo>(registerDefaultUnitOfWork: true);
        AddCapEventBus([typeof(Subscribers.DeviceAckSubscriber)]);
        services.AddScoped<Acl.IRecipeGateway, Acl.RecipeGateway>();
        services.AddScoped<Acl.IDeviceGateway, Acl.DeviceGateway>();
        services.AddScoped<Acl.IInventoryGateway, Acl.InventoryGateway>();
        services.AddScoped<Contracts.Interfaces.ICookingService, Services.CookingService>();
        services.AddHostedService<CookingSchemaHostedService>();
        services.AddHostedService<CookingWatchdogHostedService>();
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
