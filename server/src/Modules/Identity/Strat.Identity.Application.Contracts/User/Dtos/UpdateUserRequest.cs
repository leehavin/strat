namespace Strat.Identity.Application.Contracts.User.Dtos;

/// <summary>
/// 更新用户输入
/// </summary>
public class UpdateUserRequest
{
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
    public long? OrganizationId { get; set; }

    /// <summary>
    /// 角色ID
    /// </summary>
    public long? RoleId { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(500, ErrorMessage = "备注最大长度为500")]
    public string? Remark { get; set; }
}

