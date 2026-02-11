using Strat.Identity.Application.Contracts.Role.Dtos;
using Volo.Abp.Application.Services;

namespace Strat.Identity.Application.Contracts.Role;

/// <summary>
/// 角色服务接口
/// </summary>
public interface IRoleService : IApplicationService
{
    /// <summary>
    /// 分页查询角色
    /// </summary>
    Task<PagedList<RoleResponse>> GetPagedListAsync(GetRolePagedRequest input);

    /// <summary>
    /// 获取所有角色
    /// </summary>
    Task<List<RoleResponse>> GetAllAsync(string? name = null);

    /// <summary>
    /// 获取角色详情
    /// </summary>
    Task<RoleResponse> GetAsync(long id);

    /// <summary>
    /// 添加角色
    /// </summary>
    Task<long> AddAsync(AddRoleRequest input);

    /// <summary>
    /// 更新角色
    /// </summary>
    Task UpdateAsync(long id, AddRoleRequest input);

    /// <summary>
    /// 删除角色
    /// </summary>
    Task DeleteAsync(long id);

    /// <summary>
    /// 分配功能给角色
    /// </summary>
    Task AssignFunctionsAsync(AssignRoleFunctionsRequest input);

    /// <summary>
    /// 获取角色的功能ID列表
    /// </summary>
    Task<List<long>> GetFunctionIdsAsync(long roleId);
}

