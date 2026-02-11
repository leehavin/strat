using System.Text.Json.Serialization;

namespace Strat.Shared.Models
{
    /// <summary>
    /// 后端契约分页模型
    /// 基础设施层共享模型
    /// </summary>
    public class PagedResult<T>
    {
        [JsonPropertyName("pageIndex")]
        public int PageIndex { get; set; } = 1;

        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; } = 10;

        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("pageCount")]
        public int PageCount { get; set; }

        [JsonPropertyName("items")]
        public List<T> Items { get; set; } = new();
    }
}
