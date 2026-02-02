namespace Strat.Shared.Enums;

/// <summary>
/// 错误提示枚举
/// </summary>
public enum ErrorTipsEnum
{
    /// <summary>
    /// 记录不存在
    /// </summary>
    [Description("记录不存在")]
    NoResult,

    /// <summary>
    /// 无此权限
    /// </summary>
    [Description("无此权限")]
    NoPermission,

    /// <summary>
    /// 参数错误
    /// </summary>
    [Description("参数错误")]
    InvalidParameter,

    /// <summary>
    /// 操作失败
    /// </summary>
    [Description("操作失败")]
    OperationFailed
}

