namespace Strat.Shared.Enums.Workflow;

/// <summary>
/// 审批步骤类型枚举
/// </summary>
public enum AuditingStepTypeEnum
{
    /// <summary>
    /// 开始
    /// </summary>
    [Description("开始")]
    Start = 0,

    /// <summary>
    /// 审批
    /// </summary>
    [Description("审批")]
    Auditing = 1,

    /// <summary>
    /// 结束
    /// </summary>
    [Description("结束")]
    End = 2
}

