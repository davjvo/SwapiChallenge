﻿namespace StarWars.Domain.Interfaces.Caching;

public interface ICacheService
{
    Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? absoluteExpiration = null);
    T? Get<T>(string key);
    Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpiration = null);
    Task RemoveAsync(string key);
}
