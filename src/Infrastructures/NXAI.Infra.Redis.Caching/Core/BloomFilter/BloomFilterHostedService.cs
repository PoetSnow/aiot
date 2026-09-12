using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NXAI.Infra.Redis.Configurations;

namespace NXAI.Infra.Redis.Caching.Core.BloomFilter;

public class BloomFilterHostedService(IEnumerable<IBloomFilter> bloomFilters, IOptions<RedisOptions> redisOptions) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (redisOptions.Value.EnableBloomFilter && bloomFilters.IsNotNullOrEmpty())
        {
            foreach (var filter in bloomFilters)
            {
                await filter.InitAsync();
            }
        }
    }
}
