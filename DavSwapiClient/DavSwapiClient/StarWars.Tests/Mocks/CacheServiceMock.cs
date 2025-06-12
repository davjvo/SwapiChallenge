using StarWars.Domain.Interfaces.Caching;

namespace StarWars.Tests.Mocks;

public class CacheServiceMock : ICacheService
{
    public const string Starships = "starships";
    public const string Manufacturers = "manufacturers";
    Dictionary<string, object> cache = new();
    
    public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? absoluteExpiration = null)
    {
        cache.TryGetValue(key, out var cachedValue);

        if (cachedValue is null)
        {
            cachedValue = await factory();
            cache.TryAdd(key, cachedValue);
        }
        
        return (T)cachedValue;
    }

    public T? Get<T>(string key)
    {
        if(cache.TryGetValue(key, out var cachedValue)) return (T)cachedValue;

        return default;
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpiration = null)
    {
        if(value is not null) cache.TryAdd(key, value);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key)
    {
        cache.Remove(key);
        return Task.CompletedTask;
    }
}