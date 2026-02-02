namespace Strat.Identity.Application.Contracts.Auth.Dtos;

/// <summary>
/// 更新个人信息输入
/// </summary>
public class UpdateProfileRequest
{
    /// <summary>
    /// 姓名
    /// </summary>
    [MaxLength(20, ErrorMessage = "姓名最大长度为20")]
    public string? Name { get; set; }

    /// <summary>
    /// 电话
    /// </summary>
    [MaxLength(11, ErrorMessage = "电话最大长度为11")]
    public string? Telephone { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    [MaxLength(50, ErrorMessage = "邮箱最大长度为50")]
    [EmailAddress(ErrorMessage = "邮箱格式不正确")]
    public string? Email { get; set; }

    /// <summary>
    /// 新密码（为空则不修改密码）
    /// </summary>
    [MaxLength(36, ErrorMessage = "密码最大长度为36")]
    public string? Password { get; set; }
}

