namespace Strat.Shared.Enums.Workflow;

/// <summary>
/// 审批人类型枚举
/// </summary>
public enum AuditorTypeEnum
{
    /// <summary>
    /// 指定用户
    /// </summary>
    [Description("指定用户")]
    User = 0,

    /// <summary>
    /// 指定角色
    /// </summary>
    [Description("指定角色")]
    Role = 1,

    /// <summary>
    /// 部门负责人
    /// </summary>
    [Description("部门负责人")]
    DepartmentHead = 2,

    /// <summary>
    /// 发起人
    /// </summary>
    [Description("发起人")]
    Initiator = 3
}

