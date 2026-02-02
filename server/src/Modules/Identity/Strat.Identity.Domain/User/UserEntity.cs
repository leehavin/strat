using Strat.Identity.Domain.Shared;

namespace Strat.Identity.Domain.User;

/// <summary>
/// 用户实体
/// </summary>
[SugarTable("USER")]
public class UserEntity : BaseEntity
{
    /// <summary>
    /// 账号
    /// </summary>
    [SugarColumn(ColumnName = "ACCOUNT")]
    public string Account { get; set; } = string.Empty;

    /// <summary>
    /// 密码
    /// </summary>
    [SugarColumn(ColumnName = "PASSWORD")]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// 真实姓名
    /// </summary>
    [SugarColumn(ColumnName = "NAME")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 电话
    /// </summary>
    [SugarColumn(ColumnName = "TELEPHONE")]
    public string? Telephone { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    [SugarColumn(ColumnName = "EMAIL")]
    public string? Email { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    [SugarColumn(ColumnName = "AVATAR")]
    public byte[]? Avatar { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [SugarColumn(ColumnName = "STATUS")]
    public int Status { get; set; } = (int)UserStatusEnum.Normal;

    /// <summary>
    /// 组织架构ID
    /// </summary>
    [SugarColumn(ColumnName = "ORGANIZATION_ID")]
    public long OrganizationId { get; set; }

    #region 导航属性（非数据库字段）

    /// <summary>
    /// 角色ID
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public long RoleId { get; set; }

    /// <summary>
    /// 角色名称
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public string? RoleName { get; set; }

    /// <summary>
    /// 组织机构名称
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public string? OrganizationName { get; set; }

    #endregion
}

