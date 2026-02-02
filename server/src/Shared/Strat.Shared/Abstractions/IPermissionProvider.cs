using System.Collections.Generic;

namespace Strat.Shared.Abstractions;

/// <summary>
/// 权限提供者接口
/// </summary>
public interface IPermissionProvider
{
    /// <summary>
    /// 初始化权限列表
    /// </summary>
    void InitPermissions(IEnumerable<string> permissions);

    /// <summary>
    /// 检查是否有权限
    /// </summary>
    bool HasPermission(string code);

    /// <summary>
    /// 清除权限
    /// </summary>
    void Clear();
}
