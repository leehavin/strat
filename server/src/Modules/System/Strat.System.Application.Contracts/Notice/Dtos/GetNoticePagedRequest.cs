using Strat.Shared.Models;

namespace Strat.System.Application.Contracts.Notice.Dtos;

/// <summary>
/// 通知公告分页查询输入
/// </summary>
public class GetNoticePagedRequest : PagedRequest
{
    /// <summary>
    /// 标题（模糊查询）
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// 通知类型（字典ID）
    /// </summary>
    public long? NoticeType { get; set; }

    /// <summary>
    /// 级别（字典ID）
    /// </summary>
    public long? Level { get; set; }
}

