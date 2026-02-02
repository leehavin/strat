namespace Strat.Identity.Application.Contracts.Role.Dtos;

/// <summary>
/// 角色分页查询输入
/// </summary>
public class GetRolePagedRequest : PagedRequest
{
    /// <summary>
    /// 角色名称（模糊查询）
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 角色编码（模糊查询）
    /// </summary>
    public string? Code { get; set; }
}

