using Volo.Abp.DependencyInjection;

namespace Strat.Identity.Application.User;

/// <summary>
/// 用户查询服务（供其他模块调用）
/// </summary>
public class UserQueryService(ISqlSugarClient db) : IUserQueryService, ITransientDependency
{
    private readonly ISqlSugarClient _db = db;

    /// <summary>
    /// 获取用户基本信息
    /// </summary>
    public async Task<UserBasicDto?> GetBasicInfoAsync(long userId)
    {
        var user = await _db.Queryable<UserEntity>()
            .Where(u => u.Id == userId)
            .Select(u => new UserBasicDto
            {
                Id = u.Id,
                Account = u.Account,
                Name = u.Name,
                OrganizationId = u.OrganizationId
            })
            .FirstAsync();

        return user;
    }

    /// <summary>
    /// 根据角色ID获取用户ID列表
    /// </summary>
    public async Task<List<long>> GetUserIdsByRoleIdAsync(long roleId)
    {
        return await _db.Queryable<UserRoleEntity>()
            .Where(ur => ur.RoleId == roleId)
            .Select(ur => ur.UserId)
            .ToListAsync();
    }

    /// <summary>
    /// 根据组织机构ID获取用户ID列表
    /// </summary>
    public async Task<List<long>> GetUserIdsByOrganizationIdAsync(long organizationId)
    {
        return await _db.Queryable<UserEntity>()
            .Where(u => u.OrganizationId == organizationId)
            .Select(u => u.Id)
            .ToListAsync();
    }
}

