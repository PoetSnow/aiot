using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NXAI.AfterSales.Repository;
using NXAI.Infra.IdGenerater.Yitter;
using NXAI.Shared;
using NXAI.Shared.Application.Registrar;

namespace NXAI.AfterSales.Application;

/// <summary>售后模块 DI。不发 MQTT，不直接改 ast_sn。</summary>
public sealed class DependencyRegistrar(IServiceCollection services, IServiceInfo serviceInfo, IConfiguration configuration, ServiceLifetime lifetime = ServiceLifetime.Scoped)
    : AbstractApplicationDependencyRegistrar(services, serviceInfo, configuration, lifetime)
{
    protected override Assembly ApplicationLayerAssembly => Assembly.GetExecutingAssembly();

    protected override Assembly RepositoryOrDomainLayerAssembly => typeof(EntityInfo).Assembly;

    public override void AddApplicationServices()
    {
        EnsureWorkerId();
        AddOperater(services);
        AddModuleMySqlDbContext<AfterSalesDbContext, EntityInfo>(registerDefaultUnitOfWork: true);
        services.AddScoped<Acl.IAssetSnGateway, Acl.AssetSnGateway>();
        services.AddScoped<Acl.IDeviceGateway, Acl.DeviceGateway>();
        services.AddScoped<Acl.IMemberGateway, Acl.MemberGateway>();
        services.AddScoped<Contracts.Interfaces.ITicketService, Services.TicketService>();
        services.AddHostedService<AfterSalesSchemaHostedService>();
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
