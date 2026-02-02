namespace Strat.Shared.Models;

/// <summary>
/// 分页泛型集合
/// </summary>
/// <typeparam name="T">数据类型</typeparam>
public class PagedList<T>
{
    /// <summary>
    /// 页码
    /// </summary>
    public int PageIndex { get; set; } = 1;

    /// <summary>
    /// 页容量
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// 总条数
    /// </summary>
    public int Total { get; set; }

    /// <summary>
    /// 总页数
    /// </summary>
    public int PageCount => PageSize > 0 ? (int)Math.Ceiling((double)Total / PageSize) : 0;

    /// <summary>
    /// 当前页集合
    /// </summary>
    public List<T> Items { get; set; } = [];

    /// <summary>
    /// 创建分页结果
    /// </summary>
    public static PagedList<T> Create(List<T> items, int total, int pageIndex, int pageSize)
    {
        return new PagedList<T>
        {
            Items = items,
            Total = total,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }
}

/// <summary>
/// 分页扩展方法
/// </summary>
public static class PagedListExtensions
{
    /// <summary>
    /// 将列表转换为分页结果（内存分页）
    /// </summary>
    public static PagedList<T> ToPurestPagedList<T>(this List<T> source, int pageIndex, int pageSize)
    {
        var total = source.Count;
        var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        return PagedList<T>.Create(items, total, pageIndex, pageSize);
    }

    /// <summary>
    /// 将列表转换为分页结果（已分页数据）
    /// </summary>
    public static PagedList<T> ToPagedList<T>(this List<T> items, int total, int pageIndex, int pageSize)
    {
        return PagedList<T>.Create(items, total, pageIndex, pageSize);
    }
}

