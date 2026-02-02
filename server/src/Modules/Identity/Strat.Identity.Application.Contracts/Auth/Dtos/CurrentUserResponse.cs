namespace Strat.Identity.Application.Contracts.Auth.Dtos;

/// <summary>
/// 当前用户信息输出
/// </summary>
public class CurrentUserResponse
{
    /// <summary>
    /// 用户ID
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// 账号
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 真实姓名
    /// </summary>
    public string RealName { get; set; } = string.Empty;

    /// <summary>
    /// 头像
    /// </summary>
    public byte[]? Avatar { get; set; }

    /// <summary>
    /// 角色列表
    /// </summary>
    public string[] Roles { get; set; } = [];

    /// <summary>
    /// 电话
    /// </summary>
    public string? Telephone { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    public string? Email { get; set; }
}

