namespace Strat.Shared.Abstractions;

/// <summary>
/// 缓存接口
/// </summary>
public interface ICache
{
    /// <summary>
    /// 获取缓存
    /// </summary>
    T? Get<T>(string key);

    /// <summary>
    /// 获取或添加缓存
    /// </summary>
    Task<T?> GetOrAddAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiry = null);

    /// <summary>
    /// 设置缓存
    /// </summary>
    void Set<T>(string key, T value, TimeSpan? expiry = null);

    /// <summary>
    /// 移除缓存
    /// </summary>
    void Remove(string key);

    /// <summary>
    /// 移除指定前缀的缓存
    /// </summary>
    void RemoveByPrefix(string prefix);

    /// <summary>
    /// 判断缓存是否存在
    /// </summary>
    bool Exists(string key);
}

