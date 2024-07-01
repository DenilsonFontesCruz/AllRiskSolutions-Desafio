using System.Text;
using AllRiskSolutions_Desafio.Domain.ExternalServices;
using Microsoft.Extensions.Caching.Distributed;

namespace AllRiskSolutions_Desafio.Infrastructure.Services;

public class CacheManagerRedis(IDistributedCache cache) : ICacheManager
{
    public async Task SetAsync(string key, string value, TimeSpan? timeToLive = null)
    {
        await cache.SetAsync(key, Encoding.ASCII.GetBytes(value), new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = timeToLive
        });
    }

    public async Task<string> GetAsync(string key)
    {
        return Encoding.ASCII.GetString(await cache.GetAsync(key) ?? Array.Empty<byte>());
    }

    public async Task<bool> ExistsAsync(string key)
    {
        return await cache.GetAsync(key) != null;
    }
}