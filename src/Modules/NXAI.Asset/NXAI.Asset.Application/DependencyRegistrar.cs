using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NXAI.Infra.IdGenerater.Yitter;
using NXAI.Shared;
using NXAI.Shared.Application.Registrar;
using NXAI.Asset.Repository;

namespace NXAI.Asset.Application;

/// <summary>资产模块 DI。注册 DbContext 与入出库服务。</summary>
public sealed class DependencyRegistrar(IServiceCollection services, IServiceInfo serviceInfo, IConfiguration configuration, ServiceLifetime lifetime = ServiceLifetime.Scoped)
    : AbstractApplicationDependencyRegistrar(services, serviceInfo, configuration, lifetime)
{
    protected override Assembly ApplicationLayerAssembly => Assembly.GetExecutingAssembly();

    protected override Assembly RepositoryOrDomainLayerAssembly => typeof(EntityInfo).Assembly;

    public override void AddApplicationServices()
    {
        EnsureWorkerId();
        AddOperater(services);
        AddModuleMySqlDbContext<AssetDbContext, EntityInfo>(registerDefaultUnitOfWork: true);
        services.AddScoped<Acl.IMemberGateway, Acl.MemberGateway>();
        services.AddScoped<Contracts.Interfaces.IDeviceModelService, Services.DeviceModelService>();
        services.AddScoped<Contracts.Interfaces.IWarehouseService, Services.WarehouseService>();
        services.AddScoped<Contracts.Interfaces.IStockService, Services.StockService>();
        services.AddScoped<Contracts.Interfaces.IAssetSnService, Services.AssetSnService>();
        services.AddHostedService<AssetSchemaHostedService>();
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
