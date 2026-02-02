namespace Strat.System.Application.Contracts.Notice.Dtos;

/// <summary>
/// 通知公告输出
/// </summary>
public class NoticeResponse
{
    /// <summary>
    /// ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 内容
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 通知类型（字典ID）
    /// </summary>
    public long NoticeType { get; set; }

    /// <summary>
    /// 级别（字典ID）
    /// </summary>
    public long Level { get; set; }

    /// <summary>
    /// 类型名称
    /// </summary>
    public string? NoticeTypeName { get; set; }

    /// <summary>
    /// 级别名称
    /// </summary>
    public string? LevelName { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }
}

