using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using NXAI.Infra.Core.DependencyInjection;
using NXAI.Shared.WebApi.Middleware;
using NXAI.Shared.WebApi.Routing;
using Prometheus;
using Prometheus.DotNetRuntime;

namespace NXAI.Shared.WebApi.Registrar;

public abstract partial class AbstractWebApiMiddlewareRegistrar(WebApplication app)
{
    protected WebApplication App { get; init; } = app;

    /// <summary>
    /// Registers middleware.
    /// </summary>
    public abstract void UseNXAI();

    /// <summary>
    /// Registers common WebApi middleware.
    /// </summary>
    protected void UseWebApiDefault(
        Action<WebApplication>? beforeAuthentication = null,
        Action<WebApplication>? afterAuthentication = null,
        Action<WebApplication>? afterAuthorization = null,
        Action<IEndpointRouteBuilder>? endpointRoute = null)
    {
        ServiceLocator.SetProvider(App.Services);
        var environment = App.Services.GetRequiredService<IHostEnvironment>();
        var serviceInfo = App.Services.GetRequiredService<IServiceInfo>();
        //var consulOptions = App.Services.GetRequiredService<IOptions<ConsulOptions>>();
        var configuration = App.Services.GetRequiredService<IConfiguration>();
        //var healthCheckUrl = consulOptions?.Value?.HealthCheckUrl ?? $"{serviceInfo.RelativeRootPath}/health-24b01005-a76a-4b3b-8fb1-5e0f2e9564fb";
        //var defaultFilesOptions = new DefaultFilesOptions();
        //defaultFilesOptions.DefaultFileNames.Clear();
        //defaultFilesOptions.DefaultFileNames.Add("index.html");
        //App
        //    .UseDefaultFiles(defaultFilesOptions)
        //    .UseStaticFiles();
        App
            .UseStaticFiles()
            .UseRealIp(x => x.HeaderKey = "X-Forwarded-For")
            .UseCustomExceptionHandler()
            .UseCors(serviceInfo.CorsPolicy);

        if (environment.IsDevelopment())
        {
            IdentityModelEventSource.ShowPII = true;
        }

        var enableSwaggerUI = configuration.GetValue(NodeConsts.SwaggerUI_Enable, true);
        if (enableSwaggerUI)
        {
            var relativeRootPath = serviceInfo.RelativeRootPath;
            var swaggerRoutePrefix = configuration.GetValue(NodeConsts.SwaggerUI_RoutePrefix, relativeRootPath)
                ?.Trim('/')
                ?? relativeRootPath;
            var description = serviceInfo.Description;
            var includeInternal = environment.IsDevelopment();
            var surfaces = ApiSurfaces.GetSwaggerSurfaces(includeInternal);
            App
                .UseMiniProfiler()
                .UseSwagger(c =>
                {
                    c.RouteTemplate = $"/{swaggerRoutePrefix}/{{documentName}}/swagger.json";
                    c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                    {
                        swaggerDoc.Servers = [new() { Url = $"/", Description = description }];
                    });
                })
                .UseSwaggerUI(c =>
                {
                    var miniProfiler = Path.Combine(AppContext.BaseDirectory, "swagger_miniprofiler.html");
                    if (File.Exists(miniProfiler))
                    {
                        c.IndexStream = () =>
                        {
                            var text = File.ReadAllText(miniProfiler).Replace("$RELATIVEROOTPATH", relativeRootPath);
                            var byteArray = Encoding.UTF8.GetBytes(text);
                            return new MemoryStream(byteArray);
                        };
                    }

                    foreach (var surface in surfaces)
                    {
                        c.SwaggerEndpoint(
                            $"/{swaggerRoutePrefix}/{surface.GroupName}/swagger.json",
                            surface.Title);
                    }

                    c.RoutePrefix = swaggerRoutePrefix;
                });
        }
        //App
        //    .UseHealthChecks($"/{healthCheckUrl}", new HealthCheckOptions()
        //    {
        //        Predicate = _ => true,
        //        // This response outputs a JSON payload that contains the detailed results of all checks
        //        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        //    })
        //    .UseRouting();

        var enableMetrics = configuration.GetValue(NodeConsts.Metrics_Enable, false);
        if (enableMetrics)
        {
            App
                .UseHttpMetrics();

            DotNetRuntimeStatsBuilder
            .Customize()
            .WithContentionStats()
            .WithGcStats()
            .WithThreadPoolStats()
            .StartCollecting();
        }

        beforeAuthentication?.Invoke(App);
        App.UseAuthentication();
        App.UseMiddleware<ApiSurfaceTokenMiddleware>();
        afterAuthentication?.Invoke(App);
        App.UseAuthorization();
        afterAuthorization?.Invoke(App);

        App.MapControllers().RequireAuthorization();
        if (enableMetrics)
        {
            App.MapMetrics();
        }
        endpointRoute?.Invoke(App);
    }
}
