namespace Insfrastucture.Services;

public sealed class InMemoryCacheService(ILogger<InMemoryCacheService> logger) : ICacheService
{
    private readonly Dictionary<string, (object Value, DateTime Expiry)> _store = new();

    public Task<T?> GetAsync<T>(string key, CancellationToken ct = default)
    {
        if (_store.TryGetValue(key, out var entry) && entry.Expiry > DateTime.UtcNow)
        {
            logger.LogDebug("[InMemoryCache] HIT  {Key}", key);
            return Task.FromResult((T?)entry.Value);
        }

        logger.LogDebug("[InMemoryCache] MISS {Key}", key);
        return Task.FromResult(default(T?));
    }

    public Task SetAsync<T>(string key, T value, TimeSpan expiry, CancellationToken ct = default)
    {
        logger.LogDebug("[InMemoryCache] SET  {Key} (expires in {Expiry})", key, expiry);
        _store[key] = (value!, DateTime.UtcNow.Add(expiry));
        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key, CancellationToken ct = default)
    {
        logger.LogDebug("[InMemoryCache] DEL  {Key}", key);
        _store.Remove(key);
        return Task.CompletedTask;
    }
}