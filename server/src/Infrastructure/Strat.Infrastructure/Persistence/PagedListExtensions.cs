using Strat.Shared.Models;

namespace Strat.Infrastructure.Persistence;

/// <summary>
/// 分页扩展方法
/// </summary>
public static class PagedListExtensions
{
    /// <summary>
    /// 转换为分页列表
    /// </summary>
    public static async Task<PagedList<T>> ToPagedListAsync<T>(
        this ISugarQueryable<T> query,
        int pageIndex,
        int pageSize)
    {
        RefAsync<int> total = 0;
        var items = await query.ToPageListAsync(pageIndex, pageSize, total);

        return PagedList<T>.Create(items, total, pageIndex, pageSize);
    }
}

