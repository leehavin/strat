using Strat.System.Domain.Shared;

namespace Strat.System.Domain.Notice;

/// <summary>
/// 通知已读记录实体
/// </summary>
[SugarTable("NOTICE_RECORD")]
public class NoticeRecordEntity : BaseEntity
{
    /// <summary>
    /// 通知ID
    /// </summary>
    [SugarColumn(ColumnName = "NOTICE_ID")]
    public long NoticeId { get; set; }

    /// <summary>
    /// 接收人ID
    /// </summary>
    [SugarColumn(ColumnName = "RECEIVER")]
    public long Receiver { get; set; }
}

