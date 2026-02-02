namespace Strat.Identity.Application.Contracts.Auth.Dtos;

/// <summary>
/// 登录输出
/// </summary>
public class LoginResponse
{
    /// <summary>
    /// 用户ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 账号
    /// </summary>
    public string Account { get; set; } = string.Empty;

    /// <summary>
    /// 姓名
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 头像
    /// </summary>
    public byte[]? Avatar { get; set; }
}

