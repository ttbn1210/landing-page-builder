using Microsoft.Extensions.Caching.Memory;

namespace TrackQR.Web.Infrastructure.Cache;

public class MemoryCacheService
{
    private readonly IMemoryCache _cache;
    private static readonly TimeSpan DefaultExpiration = TimeSpan.FromMinutes(5);

    public MemoryCacheService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public T? Get<T>(string key)
    {
        _cache.TryGetValue(key, out T? value);
        return value;
    }

    public void Set<T>(string key, T value, TimeSpan? expiration = null)
    {
        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration ?? DefaultExpiration
        };
        _cache.Set(key, value, options);
    }

    public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null)
    {
        if (_cache.TryGetValue(key, out T? cached) && cached != null)
            return cached;

        var value = await factory();
        Set(key, value, expiration);
        return value;
    }

    public void Remove(string key)
    {
        _cache.Remove(key);
    }

    public void RemoveByPrefix(string prefix)
    {
        // MemoryCache doesn't support prefix removal natively
        // For production, consider using a wrapper that tracks keys
    }
}
