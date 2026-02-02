using Strat.Identity.Domain.Shared;

namespace Strat.Identity.Domain.Role;

/// <summary>
/// 角色实体
/// </summary>
[SugarTable("ROLE")]
public class RoleEntity : BaseEntity
{
    /// <summary>
    /// 角色名称
    /// </summary>
    [SugarColumn(ColumnName = "NAME")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 角色编码
    /// </summary>
    [SugarColumn(ColumnName = "CODE")]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// 角色描述
    /// </summary>
    [SugarColumn(ColumnName = "DESCRIPTION")]
    public string? Description { get; set; }
}

