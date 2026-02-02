using Strat.System.Domain.Shared;

namespace Strat.System.Domain.Notice;

/// <summary>
/// 通知公告实体
/// </summary>
[SugarTable("NOTICE")]
public class NoticeEntity : BaseEntity
{
    /// <summary>
    /// 标题
    /// </summary>
    [SugarColumn(ColumnName = "TITLE")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 内容
    /// </summary>
    [SugarColumn(ColumnName = "CONTENT")]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 通知类型（字典ID）
    /// </summary>
    [SugarColumn(ColumnName = "NOTICE_TYPE")]
    public long NoticeType { get; set; }

    /// <summary>
    /// 级别（字典ID）
    /// </summary>
    [SugarColumn(ColumnName = "LEVEL")]
    public long Level { get; set; }

    #region 导航属性

    [SugarColumn(IsIgnore = true)]
    public string? NoticeTypeName { get; set; }

    [SugarColumn(IsIgnore = true)]
    public string? LevelName { get; set; }

    #endregion
}

