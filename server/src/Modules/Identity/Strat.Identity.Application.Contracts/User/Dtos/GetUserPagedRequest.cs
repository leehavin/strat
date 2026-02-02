namespace Strat.Identity.Application.Contracts.User.Dtos;

/// <summary>
/// 用户分页查询输入
/// </summary>
public class GetUserPagedRequest : PagedRequest
{
    /// <summary>
    /// 姓名（模糊查询）
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 账号（模糊查询）
    /// </summary>
    public string? Account { get; set; }

    /// <summary>
    /// 电话（模糊查询）
    /// </summary>
    public string? Telephone { get; set; }

    /// <summary>
    /// 邮箱（模糊查询）
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// 状态筛选
    /// </summary>
    public int? Status { get; set; }

    /// <summary>
    /// 组织机构ID筛选
    /// </summary>
    public long? OrganizationId { get; set; }
}

