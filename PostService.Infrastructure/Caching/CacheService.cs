using PostService.Domain.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace PostService.Infrastructure.Caching;

public class CacheService<T> : ICacheService<T>
    where T : BaseObject, new()
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IDatabase _db;
    private readonly SemaphoreSlim _semaphore;
    private readonly string key = typeof(T).Name.ToLowerInvariant();

    public CacheService(IConnectionMultiplexer redis, int concurrencyLevel)
    {
        _redis = redis;
        _db = _redis.GetDatabase();
        _semaphore = new SemaphoreSlim(concurrencyLevel);
    }

    public async Task<IList<T>?> GetAllAsync()
    {
        try
        {
            _semaphore.Wait();

            var value = await _db.HashGetAllAsync(key);
            if (value.Length > 0)
            {
                var obj = Array.ConvertAll(value, val => JsonSerializer.Deserialize<T>(val.Value));
                return obj;
            }
            return null;
        }
        finally { _semaphore.Release(); }
    }

    public T? GetData(string id)
    {
        var value = _db.HashGet(key, id);
        if (!value.IsNullOrEmpty)
        {
            var obj = JsonSerializer.Deserialize<T>(value);
            return obj;
        }
        return default(T);
    }

    public bool RemovePost(string id)
    {
        bool isDeleted = _db.HashDelete(key, id);
        return isDeleted;
    }

    public T SetPost(T entity)
    {
        var expirationTime = DateTimeOffset.Now.AddMinutes(60);
        var expiration = expirationTime.DateTime.Subtract(DateTime.Now);

        var serializedObject = JsonSerializer.Serialize(entity);

        _db.HashSet(key, [
            new(entity.Id, serializedObject)
        ]);

        _db.KeyExpire(key, expiration);
        return entity;
    }
}