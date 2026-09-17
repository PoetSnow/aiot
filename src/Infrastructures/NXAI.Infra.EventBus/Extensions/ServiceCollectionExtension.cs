using System.Reflection;
using NXAI.Infra.EventBus;
using NXAI.Infra.EventBus.Cap;
using NXAI.Infra.EventBus.Cap.Filters;
using NXAI.Infra.EventBus.RabbitMq;
using DotNetCore.CAP;
using DotNetCore.CAP.Filter;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SkyApm.Diagnostics.CAP;
using SkyApm.Utilities.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddAdncInfraCap(this IServiceCollection services, IEnumerable<Type> capSubscribes, Action<CapOptions> setupAction, Action<IServiceCollection>? registrarAction = null, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        ArgumentNullException.ThrowIfNull(capSubscribes, nameof(capSubscribes));
        ArgumentNullException.ThrowIfNull(setupAction, nameof(setupAction));

        return AddAdncInfraCap<DefaultCapFilter>(services, capSubscribes, setupAction, registrarAction, serviceLifetime);
    }

    public static IServiceCollection AddAdncInfraCap<TSubscribeFilter>(this IServiceCollection services, IEnumerable<Type> capSubscribes, Action<CapOptions> setupAction, Action<IServiceCollection>? registrarAction = null, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    where TSubscribeFilter : class, ISubscribeFilter
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        ArgumentNullException.ThrowIfNull(capSubscribes, nameof(capSubscribes));
        ArgumentNullException.ThrowIfNull(setupAction, nameof(setupAction));

        // 后续模块仍要把自己的 ICapSubscribe 挂上，否则只有第一个调用方的订阅生效
        foreach (var subScriber in capSubscribes)
        {
            if (!subScriber.IsAssignableTo(typeof(ICapSubscribe)))
            {
                throw new InvalidDataException("invalid data type");
            }

            if (services.All(d => d.ServiceType != subScriber))
            {
                services.Add(new ServiceDescriptor(subScriber, subScriber, serviceLifetime));
            }
        }

        if (services.HasRegistered(nameof(AddAdncInfraCap)))
        {
            return services;
        }

        registrarAction?.Invoke(services);

        if (IsEnableSkyApm())
        {
            services.AddSkyApmExtensions().AddCap();
        }

        // ICapPublisher 是 Scoped，发布器必须同生命周期才能加入当前库事务
        services.Add(new ServiceDescriptor(typeof(IEventPublisher), typeof(CapPublisher), serviceLifetime));
        services.AddCap(setupAction).AddSubscribeFilter<TSubscribeFilter>();

        return services;
    }

    public static IServiceCollection AddNXAIInfraRabbitMq(this IServiceCollection services, Assembly? assembly, IConfigurationSection rabitmqSection, string clientProvidedName, Action<IServiceCollection>? registrarAction = null)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        ArgumentNullException.ThrowIfNull(rabitmqSection, nameof(rabitmqSection));
        ArgumentNullException.ThrowIfNullOrWhiteSpace(clientProvidedName, nameof(clientProvidedName));

        if (services.HasRegistered(nameof(AddNXAIInfraRabbitMq)))
        {
            return services;
        }

        registrarAction?.Invoke(services);
        services.Configure<RabbitMQOptions>(rabitmqSection);

        if (assembly is not null)
        {
            var serviceType = typeof(BaseRabbitMqConsumer);
            var implTypes = assembly.ExportedTypes.Where(type => type.IsAssignableTo(serviceType) && type.IsNotAbstractClass(true));
            if (implTypes is not null && implTypes.Any())
            {
                var descriptors = implTypes.Select(implType => new ServiceDescriptor(typeof(IHostedService), implType, ServiceLifetime.Singleton));
                services.TryAddEnumerable(descriptors);
            }
        }

        return services
             .AddSingleton<IConnectionManager>(provider =>
             {
                 var options = provider.GetRequiredService<IOptions<RabbitMQOptions>>();
                 var logger = provider.GetRequiredService<ILogger<ConnectionManager>>();
                 var clientName = clientProvidedName.IsNullOrWhiteSpace() ? clientProvidedName : "unknow";
                 return ConnectionManager.GetInstance(options, clientName, logger);
             })
             .AddSingleton<RabbitMqProducer>();
    }

    public static bool IsEnableSkyApm()
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_HOSTINGSTARTUPASSEMBLIES");
        if (string.IsNullOrWhiteSpace(env))
        {
            return false;
        }
        else
        {
            return env.Contains("SkyAPM.Agent.AspNetCore");
        }
    }
}
