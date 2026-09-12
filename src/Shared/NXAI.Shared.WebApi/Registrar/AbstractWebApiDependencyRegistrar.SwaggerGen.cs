using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using NXAI.Shared.WebApi.Routing;

namespace NXAI.Shared.WebApi.Registrar;

public abstract partial class AbstractWebApiDependencyRegistrar
{
    /// <summary>
    /// Registers Swagger components (one OpenAPI document per API surface).
    /// </summary>
    protected virtual void AddSwaggerGen()
    {
        var version = ServiceInfo.Version;
        var startAssemblyName = ServiceInfo.StartAssembly.GetName().Name ?? throw new InvalidDataException($"{nameof(ServiceInfo.StartAssembly)} is null");
        var includeInternal = IsDevelopmentEnvironment();
        var surfaces = ApiSurfaces.GetSwaggerSurfaces(includeInternal);
        //Services.AddEndpointsApiExplorer();
        Services
            .AddSwaggerGen(c =>
            {
                foreach (var surface in surfaces)
                {
                    c.SwaggerDoc(surface.GroupName, new OpenApiInfo
                    {
                        Title = $"{ServiceInfo.ShortName} - {surface.Title}",
                        Version = version,
                        Description = surface.RoutePrefix
                    });
                }

                c.DocInclusionPredicate((documentName, apiDescription) =>
                    string.Equals(apiDescription.GroupName, documentName, StringComparison.OrdinalIgnoreCase));

                // Use bearer token authentication
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                // Set global authentication
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                var lastName = startAssemblyName.Split('.').Last();
                var apiLayerXmlFilePath = Path.Combine(AppContext.BaseDirectory, $"{startAssemblyName}.xml");
                var applicationContractsLayerXmlFilePath = Path.Combine(AppContext.BaseDirectory, $"{startAssemblyName.Replace($".{lastName}", ".Application.Contracts")}.xml");
                var applicationLayerXmlFilePath = Path.Combine(AppContext.BaseDirectory, $"{startAssemblyName.Replace($".{lastName}", ".Application")}.xml");
                var includedXml = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                void IncludeXmlIfExists(string path)
                {
                    if (!includedXml.Add(path) || !File.Exists(path))
                    {
                        return;
                    }

                    c.IncludeXmlComments(path, true);
                }

                IncludeXmlIfExists(apiLayerXmlFilePath);
                IncludeXmlIfExists(applicationContractsLayerXmlFilePath);
                IncludeXmlIfExists(applicationLayerXmlFilePath);
                IncludeXmlIfExists(Path.Combine(AppContext.BaseDirectory, "NXAI.Shared.Application.Contracts.xml"));

                foreach (var assembly in GetModuleApiAssemblies())
                {
                    var moduleName = assembly.GetName().Name;
                    if (string.IsNullOrWhiteSpace(moduleName))
                    {
                        continue;
                    }

                    IncludeXmlIfExists(Path.Combine(AppContext.BaseDirectory, $"{moduleName}.xml"));
                    if (moduleName.EndsWith(".API", StringComparison.Ordinal))
                    {
                        var contractsXml = Path.Combine(
                            AppContext.BaseDirectory,
                            $"{moduleName[..^4]}.Application.Contracts.xml");
                        IncludeXmlIfExists(contractsXml);
                    }
                }
            })
            .AddFluentValidationRulesToSwagger();
    }

    protected virtual bool IsDevelopmentEnvironment()
    {
        var environmentName = Configuration["ASPNETCORE_ENVIRONMENT"]
            ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        return string.Equals(environmentName, Environments.Development, StringComparison.OrdinalIgnoreCase);
    }
}
