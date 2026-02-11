using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Strat.Shared.Extensions;

namespace Strat.Identity.Application.Auth;

/// <summary>
/// 认证服务
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroupConst.System)]
[Authorize]
public class AuthService(
    ISqlSugarClient db,
    IHttpContextAccessor httpContextAccessor,
    ITokenService tokenService,
    ICurrentUser currentUser,
    ICache cache) : ApplicationService, IAuthService
{
    private readonly ISqlSugarClient _db = db;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly ITokenService _tokenService = tokenService;
    private readonly ICurrentUser _currentUser = currentUser;
    private readonly ICache _cache = cache;

    private const string PermissionCacheKey = "Auth:Permissions:{0}";
    private const string RouterCacheKey = "Auth:Routers:{0}";

    /// <summary>
    /// 用户登录
    /// </summary>
    [AllowAnonymous]
    public async Task<LoginResponse> LoginAsync(LoginRequest input)
    {
        var user = await _db.Queryable<UserEntity>()
            .FirstAsync(u => u.Account == input.Account);

        if (user == null)
            throw BusinessException.Throw("用户名或密码错误");

        // 验证密码
        //if (!BCryptHelper.VerifyPassword(input.Password, user.Password))
        //    throw BusinessException.Throw("用户名或密码错误");

        // 检查用户状态
        if (user.Status != (int)UserStatusEnum.Normal)
            throw BusinessException.Throw("账号状态异常，请联系管理员");

        // 生成 Token
        var claims = new[]
        {
            new Claim(ClaimConst.UserId, user.Id.ToString()),
            new Claim(ClaimConst.UserName, user.Name),
            new Claim(ClaimConst.OrganizationId, user.OrganizationId.ToString())
        };

        var token = _tokenService.GenerateToken(claims);

        // 设置响应头
        _httpContextAccessor.HttpContext?.Response.Headers.Append("accesstoken", token);

        return user.Adapt<LoginResponse>();
    }

    /// <summary>
    /// 获取当前用户信息
    /// </summary>
    public async Task<CurrentUserResponse> GetCurrentUserAsync()
    {
        var user = await _db.Queryable<UserEntity>()
            .FirstAsync(u => u.Id == _currentUser.UserId);

        if (user == null)
            throw BusinessException.Throw(ErrorTipsEnum.NoResult);

        var roles = await _db.Queryable<RoleEntity>()
            .InnerJoin<UserRoleEntity>((r, ur) => r.Id == ur.RoleId)
            .Where((r, ur) => ur.UserId == user.Id)
            .Select((r, ur) => r.Code)
            .ToListAsync();

        return new CurrentUserResponse
        {
            UserId = user.Id.ToString(),
            UserName = user.Account,
            RealName = user.Name,
            Avatar = user.Avatar,
            Telephone = user.Telephone,
            Email = user.Email,
            Roles = roles.ToArray()
        };
    }

    /// <summary>
    /// 获取当前用户权限列表
    /// </summary>
    public async Task<List<string>> GetPermissionsAsync()
    {
        var cacheKey = string.Format(PermissionCacheKey, _currentUser.UserId);
        return await _cache.GetOrAddAsync(cacheKey, async () =>
        {
            return await _db.Queryable<FunctionEntity>()
                .InnerJoin<RoleFunctionEntity>((f, rf) => f.Id == rf.FunctionId)
                .InnerJoin<UserRoleEntity>((f, rf, ur) => rf.RoleId == ur.RoleId)
                .Where((f, rf, ur) => ur.UserId == _currentUser.UserId)
                .Select((f, rf, ur) => f.Code)
                .Distinct()
                .ToListAsync();
        }, TimeSpan.FromHours(2)) ?? [];
    }

    /// <summary>
    /// 获取当前用户路由
    /// </summary>
    public async Task<List<RouterResponse>> GetRoutersAsync()
    {
        var cacheKey = string.Format(RouterCacheKey, _currentUser.UserId);
        return await _cache.GetOrAddAsync(cacheKey, async () =>
        {
            var functions = await _db.Queryable<FunctionEntity>()
                .InnerJoin<RoleFunctionEntity>((f, rf) => f.Id == rf.FunctionId)
                .InnerJoin<UserRoleEntity>((f, rf, ur) => rf.RoleId == ur.RoleId)
                .Where((f, rf, ur) => ur.UserId == _currentUser.UserId)
                .Where((f, rf, ur) => f.Type != (int)FunctionTypeEnum.Button)
                .OrderBy((f, rf, ur) => f.Sort)
                .Select((f, rf, ur) => f)
                .Distinct()
                .ToListAsync();

            var list = functions.Adapt<List<RouterResponse>>();
            return list.ToTree(x => x.Children, x => x.ParentId, 0L);
        }, TimeSpan.FromHours(2)) ?? [];
    }

    /// <summary>
    /// 更新个人信息
    /// </summary>
    public async Task UpdateProfileAsync(UpdateProfileRequest input)
    {
        var user = await _db.Queryable<UserEntity>()
            .FirstAsync(u => u.Id == _currentUser.UserId);

        if (user == null)
            throw BusinessException.Throw(ErrorTipsEnum.NoResult);

        if (input.Name.IsNotNullOrWhiteSpace())
            user.Name = input.Name!;

        if (input.Telephone.IsNotNullOrWhiteSpace())
            user.Telephone = input.Telephone;

        if (input.Email.IsNotNullOrWhiteSpace())
            user.Email = input.Email;

        if (input.Password.IsNotNullOrWhiteSpace())
            user.Password = BCryptHelper.HashPassword(input.Password!);

        await _db.Updateable(user)
            .IgnoreColumns(u => new { u.Account, u.Status, u.CreateBy, u.CreateTime })
            .ExecuteCommandAsync();
    }
}

