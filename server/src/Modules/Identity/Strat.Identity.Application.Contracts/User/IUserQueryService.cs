using Strat.Identity.Application.Contracts.User.Dtos;

namespace Strat.Identity.Application.Contracts.User;

/// <summary>
/// 用户查询服务接口（供其他模块调用）
/// </summary>
public interface IUserQueryService
{
    /// <summary>
    /// 获取用户基本信息
    /// </summary>
    Task<UserBasicDto?> GetBasicInfoAsync(long userId);

    /// <summary>
    /// 根据角色ID获取用户ID列表
    /// </summary>
    Task<List<long>> GetUserIdsByRoleIdAsync(long roleId);

    /// <summary>
    /// 根据组织机构ID获取用户ID列表
    /// </summary>
    Task<List<long>> GetUserIdsByOrganizationIdAsync(long organizationId);
}

