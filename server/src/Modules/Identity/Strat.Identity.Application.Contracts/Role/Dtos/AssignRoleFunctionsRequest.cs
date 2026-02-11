namespace Strat.Identity.Application.Contracts.Role.Dtos;

/// <summary>
/// 分配角色功能请求
/// </summary>
public class AssignRoleFunctionsRequest
{
    /// <summary>
    /// 角色ID
    /// </summary>
    public long RoleId { get; set; }

    /// <summary>
    /// 功能ID列表
    /// </summary>
    public List<long> FunctionIds { get; set; } = new();
}
