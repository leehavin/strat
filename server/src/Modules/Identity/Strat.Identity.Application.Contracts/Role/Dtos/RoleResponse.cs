namespace Strat.Identity.Application.Contracts.Role.Dtos;

/// <summary>
/// 角色输出
/// </summary>
public class RoleResponse
{
    /// <summary>
    /// 角色ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 角色名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 角色编码
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// 角色描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }
}

