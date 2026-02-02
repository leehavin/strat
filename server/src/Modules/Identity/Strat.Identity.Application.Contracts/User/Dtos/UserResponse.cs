namespace Strat.Identity.Application.Contracts.User.Dtos;

/// <summary>
/// 用户输出
/// </summary>
public class UserResponse
{
    /// <summary>
    /// 主键ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 账号
    /// </summary>
    public string Account { get; set; } = string.Empty;

    /// <summary>
    /// 真实姓名
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 电话
    /// </summary>
    public string? Telephone { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    public byte[]? Avatar { get; set; }

    /// <summary>
    /// 用户状态
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// 组织机构ID
    /// </summary>
    public long OrganizationId { get; set; }

    /// <summary>
    /// 组织机构名称
    /// </summary>
    public string? OrganizationName { get; set; }

    /// <summary>
    /// 角色ID
    /// </summary>
    public long RoleId { get; set; }

    /// <summary>
    /// 角色名称
    /// </summary>
    public string? RoleName { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }
}

