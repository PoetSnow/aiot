using Microsoft.Extensions.Configuration;
using NXAI.Infra.Redis.Caching;
using NXAI.Infra.Redis.Caching.Configurations;
using NXAI.Infra.Redis.Caching.Core.BloomFilter;
using NXAI.Infra.Redis.Caching.Core.Diagnostics.SkyApm;
using NXAI.Infra.Redis.Caching.Core.Interceptor;
using NXAI.Infra.Redis.Caching.Core.Interceptor.Castle;
using NXAI.Infra.Redis.Caching.Core.Preheater;
using NXAI.Infra.Redis.Caching.Provider;
using SkyApm;
using SkyApm.Utilities.DependencyInjection;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddAdncInfraRedisCaching(this IServiceCollection services, Assembly? assembly, IConfigurationSection redisSection, IConfigurationSection cachingSection, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        ArgumentNullException.ThrowIfNull(redisSection, nameof(redisSection));
        ArgumentNullException.ThrowIfNull(cachingSection, nameof(cachingSection));

        if (services.HasRegistered(nameof(AddAdncInfraRedisCaching)))
        {
            return services;
        }

        if (IsEnableSkyApm())
        {
            services.AddSkyApmExtensions().Services.AddSingleton<ITracingDiagnosticProcessor, CacheTracingDiagnosticProcessor>();
        }

        if (assembly is not null)
        {
            var preheatableImpl = assembly.ExportedTypes.SingleOrDefault(type => type.IsAssignableTo(typeof(ICachePreheatable)) && type.IsNotAbstractClass(true));
            if (preheatableImpl is not null)
            {
                services
                    .AddSingleton(preheatableImpl)
                    .AddSingleton(x => (ICachePreheatable)x.GetRequiredService(preheatableImpl))
                    .AddHostedService<CachingHostedService>();
            }

            var bloomFilterType = typeof(IBloomFilter);
            var bloomFilterImpls = assembly.ExportedTypes.Where(type => type.IsAssignableTo(bloomFilterType) && type.IsNotAbstractClass(true)).ToList();
            if (bloomFilterImpls.IsNotNullOrEmpty())
            {
                bloomFilterImpls.ForEach(implType => services.AddSingleton(bloomFilterType, implType));

                services
                    .AddSingleton<IBloomFilter, NullBloomFilter>()
                    .AddSingleton<BloomFilterFactory>()
                    .AddHostedService<BloomFilterHostedService>();
            }
        }

        services
            .AddNXAIInfraRedis(redisSection)
            .Configure<CacheOptions>(cachingSection)
            .AddSingleton<ICacheProvider, DefaultCachingProvider>()
            .AddSingleton<ICachingKeyGenerator, DefaultCachingKeyGenerator>();

        services.Add(new ServiceDescriptor(typeof(CachingInterceptor), typeof(CachingInterceptor), serviceLifetime));
        services.Add(new ServiceDescriptor(typeof(CachingAsyncInterceptor), typeof(CachingAsyncInterceptor), serviceLifetime));

        return services;
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
