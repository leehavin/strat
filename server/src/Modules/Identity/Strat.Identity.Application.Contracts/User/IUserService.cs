using Strat.Identity.Application.Contracts.User.Dtos;
using Volo.Abp.Application.Services;

namespace Strat.Identity.Application.Contracts.User;

/// <summary>
/// 用户服务接口
/// </summary>
public interface IUserService : IApplicationService
{
    /// <summary>
    /// 分页查询用户
    /// </summary>
    Task<PagedList<UserResponse>> GetPagedListAsync(GetUserPagedRequest input);

    /// <summary>
    /// 获取用户详情
    /// </summary>
    Task<UserResponse> GetAsync(long id);

    /// <summary>
    /// 添加用户
    /// </summary>
    Task<long> AddAsync(AddUserRequest input);

    /// <summary>
    /// 更新用户
    /// </summary>
    Task UpdateAsync(long id, UpdateUserRequest input);

    /// <summary>
    /// 删除用户
    /// </summary>
    Task DeleteAsync(long id);

    /// <summary>
    /// 停用用户
    /// </summary>
    Task DisableAsync(long id);

    /// <summary>
    /// 启用用户
    /// </summary>
    Task EnableAsync(long id);

    /// <summary>
    /// 重置密码
    /// </summary>
    Task<string> ResetPasswordAsync(long id);
}

