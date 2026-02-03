namespace Strat.Identity.Application.Contracts.Auth.Dtos;

/// <summary>
/// 登录输入
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// 账号
    /// </summary>
    [Required(ErrorMessage = "账号不能为空")]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 密码
    /// </summary>
    [Required(ErrorMessage = "密码不能为空")]
    public string Password { get; set; } = string.Empty;
}

