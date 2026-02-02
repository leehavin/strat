using Strat.Identity.Domain.Shared;

namespace Strat.Identity.Domain.Role;

/// <summary>
/// 角色功能关联实体
/// </summary>
[SugarTable("ROLE_FUNCTION")]
public class RoleFunctionEntity : BaseEntity
{
    /// <summary>
    /// 角色ID
    /// </summary>
    [SugarColumn(ColumnName = "ROLE_ID")]
    public long RoleId { get; set; }

    /// <summary>
    /// 功能ID
    /// </summary>
    [SugarColumn(ColumnName = "FUNCTION_ID")]
    public long FunctionId { get; set; }
}

