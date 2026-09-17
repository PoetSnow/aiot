namespace NXAI.Shared.WebApi.Routing;

/// <summary>三端路由前缀与 Swagger 分组。按调用方分，不按模块分。</summary>
public static class ApiSurfaces
{
    /// <summary>后台管理文档组。</summary>
    public const string ConsoleGroup = "console";

    /// <summary>小程序文档组。</summary>
    public const string PortalGroup = "portal";

    /// <summary>设备端文档组。</summary>
    public const string DeviceGroup = "device";

    /// <summary>内部调试文档组，仅开发环境。</summary>
    public const string InternalGroup = "internal";

    /// <summary>后台管理路由前缀。</summary>
    public const string ConsoleRoutePrefix = "api/console";

    /// <summary>小程序路由前缀。</summary>
    public const string PortalRoutePrefix = "api/portal";

    /// <summary>设备端路由前缀。</summary>
    public const string DeviceRoutePrefix = "api/device";

    /// <summary>内部调试路由前缀。</summary>
    public const string InternalRoutePrefix = "api/internal";

    /// <summary>后台 Controller 默认路由模板。</summary>
    public const string ConsoleRouteTemplate = $"{ConsoleRoutePrefix}/[controller]";

    /// <summary>小程序 Controller 默认路由模板。</summary>
    public const string PortalRouteTemplate = $"{PortalRoutePrefix}/[controller]";

    /// <summary>设备端 Controller 默认路由模板。</summary>
    public const string DeviceRouteTemplate = $"{DeviceRoutePrefix}/[controller]";

    /// <summary>内部调试 Controller 默认路由模板。</summary>
    public const string InternalRouteTemplate = $"{InternalRoutePrefix}/[controller]";

    /// <summary>按端返回 Swagger 文档组。</summary>
    /// <param name="includeInternal">开发环境才带内部调试组。</param>
    public static IReadOnlyList<ApiSurfaceInfo> GetSwaggerSurfaces(bool includeInternal)
    {
        var surfaces = new List<ApiSurfaceInfo>
        {
            new(ConsoleGroup, "后台管理 API", ConsoleRoutePrefix),
            new(PortalGroup, "小程序 API", PortalRoutePrefix),
            new(DeviceGroup, "设备端 API", DeviceRoutePrefix),
        };

        if (includeInternal)
        {
            surfaces.Add(new(InternalGroup, "内部调试 API", InternalRoutePrefix, DevelopmentOnly: true));
        }

        return surfaces;
    }
}

/// <summary>Swagger 文档组。</summary>
/// <param name="GroupName">文档组名，写入 JWT aud。</param>
/// <param name="Title">中文标题。</param>
/// <param name="RoutePrefix">该组路由前缀。</param>
/// <param name="DevelopmentOnly">是否仅开发环境暴露。</param>
public sealed record ApiSurfaceInfo(
    string GroupName,
    string Title,
    string RoutePrefix,
    bool DevelopmentOnly = false);
