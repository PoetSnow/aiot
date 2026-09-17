using System.Reflection;
using NXAI.Host.Auth;
using NXAI.Shared;
using NXAI.Shared.WebApi.Registrar;

namespace NXAI.Host.Registrar;

public sealed class WebApiDependencyRegistrar(IServiceCollection services, IServiceInfo serviceInfo, IConfiguration configuration)
    : AbstractWebApiDependencyRegistrar(services, serviceInfo, configuration)
{
    protected override IEnumerable<Assembly> GetModuleApiAssemblies() =>
    [
        typeof(NXAI.System.API.RouteConsts).Assembly,
        typeof(NXAI.Member.API.Controllers.MemberPingController).Assembly,
        typeof(NXAI.Asset.API.Controllers.AssetPingController).Assembly,
        typeof(NXAI.AfterSales.API.Controllers.AfterSalesPingController).Assembly,
        typeof(NXAI.Device.API.Controllers.DevicePingController).Assembly,
        typeof(NXAI.Recipe.API.Controllers.RecipePingController).Assembly,
        typeof(NXAI.Cooking.API.Controllers.CookingPingController).Assembly,
        typeof(NXAI.Inventory.API.Controllers.InventoryPingController).Assembly,
        typeof(NXAI.Catalog.API.Controllers.CatalogPingController).Assembly,
    ];

    public override void AddAdncServices()
    {
        AddWebApiDefaultServices<SurfaceAwareAuthenticationProcessor, DeferredPermissionHandler>();
        services.AddScoped<UserContext>();
        services.AddSingleton<IServiceInfo>(serviceInfo);
        services.AddNXAIApplicationModules(
            serviceInfo,
            configuration,
            typeof(NXAI.Member.Application.DependencyRegistrar).Assembly,
            typeof(NXAI.Asset.Application.DependencyRegistrar).Assembly,
            typeof(NXAI.AfterSales.Application.DependencyRegistrar).Assembly,
            typeof(NXAI.Device.Application.DependencyRegistrar).Assembly,
            typeof(NXAI.Recipe.Application.DependencyRegistrar).Assembly,
            typeof(NXAI.Cooking.Application.DependencyRegistrar).Assembly,
            typeof(NXAI.Inventory.Application.DependencyRegistrar).Assembly,
            typeof(NXAI.Catalog.Application.DependencyRegistrar).Assembly);
    }
}
