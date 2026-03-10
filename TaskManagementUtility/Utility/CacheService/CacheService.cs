
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace TaskManagementSystem.Utility.CacheService;

public class CacheService : ICacheService
{
    private readonly IDistributedCache _distributedCache;
    public CacheService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task<T> GetAsync<T>(string key)
    {
         var data = await _distributedCache.GetStringAsync(key);
        if (data != null)
        {
              return JsonSerializer.Deserialize<T>(data);
        }
        return default;
    }

    public async Task RemoveAsync(string key)
    {
        await _distributedCache.RemoveAsync(key);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration
        };
        var data = JsonSerializer.Serialize(value);
        await _distributedCache.SetStringAsync(key, data, options);
    }
}
