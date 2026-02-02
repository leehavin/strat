using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Memory;
using Strat.Shared.Abstractions;

namespace Strat.Infrastructure.Cache;

/// <summary>
/// 内存缓存实现
/// </summary>
public class MemoryCache(Microsoft.Extensions.Caching.Memory.IMemoryCache memoryCache) : ICache, ISingletonDependency
{
    private readonly IMemoryCache _cache = memoryCache;
    private readonly ConcurrentDictionary<string, byte> _keys = new();

    /// <summary>
    /// 获取缓存
    /// </summary>
    public T? Get<T>(string key)
    {
        return _cache.Get<T>(key);
    }

    /// <summary>
    /// 获取或添加缓存
    /// </summary>
    public async Task<T?> GetOrAddAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiry = null)
    {
        if (_cache.TryGetValue(key, out T? value))
            return value;

        value = await factory();

        if (value != null)
        {
            Set(key, value, expiry);
        }

        return value;
    }

    /// <summary>
    /// 设置缓存
    /// </summary>
    public void Set<T>(string key, T value, TimeSpan? expiry = null)
    {
        var options = new MemoryCacheEntryOptions();

        if (expiry.HasValue)
            options.SetAbsoluteExpiration(expiry.Value);

        _cache.Set(key, value, options);
        _keys.TryAdd(key, 0);
    }

    /// <summary>
    /// 移除缓存
    /// </summary>
    public void Remove(string key)
    {
        _cache.Remove(key);
        _keys.TryRemove(key, out _);
    }

    /// <summary>
    /// 移除指定前缀的缓存
    /// </summary>
    public void RemoveByPrefix(string prefix)
    {
        var keysToRemove = _keys.Keys.Where(k => k.StartsWith(prefix)).ToList();
        foreach (var key in keysToRemove)
        {
            Remove(key);
        }
    }

    /// <summary>
    /// 判断缓存是否存在
    /// </summary>
    public bool Exists(string key)
    {
        return _cache.TryGetValue(key, out _);
    }
}

