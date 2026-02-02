namespace Strat.Identity.Application.Contracts.User.Dtos;

/// <summary>
/// 用户基本信息（供其他模块调用）
/// </summary>
public class UserBasicDto
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
    /// 组织机构ID
    /// </summary>
    public long OrganizationId { get; set; }
}

