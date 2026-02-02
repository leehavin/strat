using Strat.Identity.Application.Contracts.Auth.Dtos;
using Volo.Abp.Application.Services;

namespace Strat.Identity.Application.Contracts.Auth;

/// <summary>
/// 认证服务接口
/// </summary>
public interface IAuthService : IApplicationService
{
    /// <summary>
    /// 用户登录
    /// </summary>
    Task<LoginResponse> LoginAsync(LoginRequest input);

    /// <summary>
    /// 获取当前用户信息
    /// </summary>
    Task<CurrentUserResponse> GetCurrentUserAsync();

    /// <summary>
    /// 获取当前用户权限列表
    /// </summary>
    Task<List<string>> GetPermissionsAsync();

    /// <summary>
    /// 获取当前用户路由
    /// </summary>
    Task<List<RouterResponse>> GetRoutersAsync();

    /// <summary>
    /// 更新个人信息
    /// </summary>
    Task UpdateProfileAsync(UpdateProfileRequest input);
}

