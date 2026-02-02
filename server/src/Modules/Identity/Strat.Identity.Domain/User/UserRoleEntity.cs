using Strat.Identity.Domain.Shared;

namespace Strat.Identity.Domain.User;

/// <summary>
/// 用户角色关联实体
/// </summary>
[SugarTable("USER_ROLE")]
public class UserRoleEntity : BaseEntity
{
    /// <summary>
    /// 用户ID
    /// </summary>
    [SugarColumn(ColumnName = "USER_ID")]
    public long UserId { get; set; }

    /// <summary>
    /// 角色ID
    /// </summary>
    [SugarColumn(ColumnName = "ROLE_ID")]
    public long RoleId { get; set; }
}

