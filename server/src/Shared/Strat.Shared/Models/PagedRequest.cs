using System.ComponentModel.DataAnnotations;

namespace Strat.Shared.Models;

/// <summary>
/// 分页查询基类
/// </summary>
public class PagedRequest
{
    private int _pageIndex = 1;
    private int _pageSize = 10;

    /// <summary>
    /// 页码
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "页码最小为1")]
    public int PageIndex
    {
        get => _pageIndex;
        set => _pageIndex = value < 1 ? 1 : value;
    }

    /// <summary>
    /// 页容量
    /// </summary>
    [Range(1, 100, ErrorMessage = "页容量在1-100之间")]
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value < 1 ? 10 : (value > 100 ? 100 : value);
    }
}

