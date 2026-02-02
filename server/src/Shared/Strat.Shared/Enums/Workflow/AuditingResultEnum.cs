namespace Strat.Shared.Enums.Workflow;

/// <summary>
/// 审批结果枚举
/// </summary>
public enum AuditingResultEnum
{
    /// <summary>
    /// 待审批
    /// </summary>
    [Description("待审批")]
    Pending = 0,

    /// <summary>
    /// 通过
    /// </summary>
    [Description("通过")]
    Approved = 1,

    /// <summary>
    /// 拒绝
    /// </summary>
    [Description("拒绝")]
    Rejected = 2,

    /// <summary>
    /// 退回
    /// </summary>
    [Description("退回")]
    Returned = 3
}

