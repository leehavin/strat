using Microsoft.AspNetCore.Http;
using Strat.Shared.Abstractions;
using Strat.Shared.Constants;

namespace Strat.Infrastructure.Auth;

/// <summary>
/// 当前用户实现
/// </summary>
public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser, ITransientDependency
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    /// <summary>
    /// 用户ID
    /// </summary>
    public long UserId
    {
        get
        {
            var claim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimConst.UserId);
            return (claim != null && long.TryParse(claim.Value, out var id)) ? id : 0;
        }
    }

    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName
    {
        get
        {
            var claim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimConst.UserName);
            return claim?.Value ?? string.Empty;
        }
    }

    /// <summary>
    /// 组织机构ID
    /// </summary>
    public long OrganizationId
    {
        get
        {
            var claim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimConst.OrganizationId);
            return (claim != null && long.TryParse(claim.Value, out var id)) ? id : 0;
        }
    }

    /// <summary>
    /// 是否已认证
    /// </summary>
    public bool IsAuthenticated =>
        _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
}

