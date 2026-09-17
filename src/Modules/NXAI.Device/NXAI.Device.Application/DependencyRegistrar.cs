using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NXAI.Device.Repository;
using NXAI.Infra.IdGenerater.Yitter;
using NXAI.Shared;
using NXAI.Shared.Application.Registrar;

namespace NXAI.Device.Application;

/// <summary>设备模块 DI。注册绑定、影子、Outbox、MQTT 工作器与遥测。</summary>
public sealed class DependencyRegistrar(IServiceCollection services, IServiceInfo serviceInfo, IConfiguration configuration, ServiceLifetime lifetime = ServiceLifetime.Scoped)
    : AbstractApplicationDependencyRegistrar(services, serviceInfo, configuration, lifetime)
{
    protected override Assembly ApplicationLayerAssembly => Assembly.GetExecutingAssembly();

    protected override Assembly RepositoryOrDomainLayerAssembly => typeof(EntityInfo).Assembly;

    public override void AddApplicationServices()
    {
        EnsureWorkerId();
        AddOperater(services);
        AddModuleMySqlDbContext<DeviceDbContext, EntityInfo>(registerDefaultUnitOfWork: true);
        services.AddScoped<Acl.IAssetSnGateway, Acl.AssetSnGateway>();
        services.AddScoped<Acl.IMemberGateway, Acl.MemberGateway>();
        services.AddScoped<Contracts.Interfaces.IInventoryQuery, Acl.DeferredInventoryQuery>();
        services.AddSingleton<Contracts.Interfaces.ITelemetryStore, Stores.RelationalTelemetryStore>();
        services.AddScoped<Contracts.Interfaces.IDeviceService, Services.DeviceService>();
        services.AddHostedService<DeviceSchemaHostedService>();
        services.AddHostedService<Mqtt.DeviceMqttHostedService>();
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
