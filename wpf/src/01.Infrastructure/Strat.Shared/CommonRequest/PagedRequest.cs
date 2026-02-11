using System.ComponentModel.DataAnnotations;

namespace Strat.Shared.CommonRequest
{
    public class PagedRequest
    {
        /// <summary>
        /// 页码
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "页码最小为1")]
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 页容量
        /// </summary>
        [Range(1, 100, ErrorMessage = "页容量在1-100之间")]
        public int PageSize { get; set; } = 10;
    }
}


