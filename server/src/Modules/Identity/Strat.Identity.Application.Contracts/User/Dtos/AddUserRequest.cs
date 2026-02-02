namespace Strat.Identity.Application.Contracts.User.Dtos;

/// <summary>
/// 添加用户输入
/// </summary>
public class AddUserRequest
{
    /// <summary>
    /// 账号
    /// </summary>
    [Required(ErrorMessage = "账号不能为空")]
    [MaxLength(36, ErrorMessage = "账号最大长度为36")]
    public string Account { get; set; } = string.Empty;

    /// <summary>
    /// 密码（为空则使用默认密码）
    /// </summary>
    [MaxLength(36, ErrorMessage = "密码最大长度为36")]
    public string? Password { get; set; }

    /// <summary>
    /// 真实姓名
    /// </summary>
    [MaxLength(20, ErrorMessage = "姓名最大长度为20")]
    public string? Name { get; set; }

    /// <summary>
    /// 电话
    /// </summary>
    [MaxLength(11, ErrorMessage = "电话最大长度为11")]
    [RegularExpression(@"^1[3-9]\d{9}$", ErrorMessage = "手机号格式不正确")]
    public string? Telephone { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    [MaxLength(50, ErrorMessage = "邮箱最大长度为50")]
    [EmailAddress(ErrorMessage = "邮箱格式不正确")]
    public string? Email { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    public byte[]? Avatar { get; set; }

    /// <summary>
    /// 组织机构ID
    /// </summary>
    [Required(ErrorMessage = "组织机构不能为空")]
    public long OrganizationId { get; set; }

    /// <summary>
    /// 角色ID
    /// </summary>
    [Required(ErrorMessage = "角色不能为空")]
    public long RoleId { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(500, ErrorMessage = "备注最大长度为500")]
    public string? Remark { get; set; }
}

