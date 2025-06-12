using Microsoft.Extensions.Caching.Memory;
using StarWars.Domain.Interfaces.Caching;

namespace StarWars.Infrastructure.Caching;

public class MemoryCacheService(IMemoryCache memoryCache) : ICacheService
{
    public const string Starships = "starships";
    public const string Manufacturers = "manufacturers";
    private readonly TimeSpan _defaultExpiration = TimeSpan.FromMinutes(60);

    public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? absoluteExpiration)
    {
        if (memoryCache.TryGetValue<T>(key, out var cachedValue) && cachedValue is not null)
            return cachedValue;

        var result = await factory();
        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = absoluteExpiration ?? _defaultExpiration
        };

        memoryCache.Set(key, result, options);

        return result;
    }

    public T? Get<T>(string key)
    {
        memoryCache.TryGetValue<T?>(key, out var cachedValue);
        return cachedValue;
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpiration)
    {
        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = absoluteExpiration ?? _defaultExpiration
        };
        memoryCache.Set(key, value, options);

        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key)
    {
        memoryCache.Remove(key);
        return Task.CompletedTask;
    }
}
