using System.Collections.Generic;

namespace Strat.Shared.Models
{
    /// <summary>
    /// 后端契约分页模型
    /// 基础设施层共享模型
    /// </summary>
    public class PagedResult<T>
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int Total { get; set; }
        public int PageCount { get; set; }
        public List<T> Items { get; set; } = new();
    }
}
