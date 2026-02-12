using Strat.Shared.Extensions;

namespace Strat.Identity.Application.Role;

/// <summary>
/// 角色服务
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroupConst.System)]
[Authorize]
public class RoleService(ISqlSugarClient db, ICache cache) : ApplicationService, IRoleService
{
    private readonly ISqlSugarClient _db = db;
    private readonly ICache _cache = cache;

    private const string PermissionCachePrefix = "Auth:Permissions:";
    private const string RouterCachePrefix = "Auth:Routers:";

    /// <summary>
    /// 分页查询角色
    /// </summary>
    public async Task<PagedList<RoleResponse>> GetPagedListAsync(GetRolePagedRequest input)
    {
        RefAsync<int> total = 0;

        var list = await _db.Queryable<RoleEntity>()
            .WhereIF(input.Name.IsNotNullOrWhiteSpace(), r => r.Name.Contains(input.Name!))
            .WhereIF(input.Code.IsNotNullOrWhiteSpace(), r => r.Code.Contains(input.Code!))
            .OrderByDescending(r => r.CreateTime)
            .ToPageListAsync(input.PageIndex, input.PageSize, total);

        var items = list.Adapt<List<RoleResponse>>();
        return PagedList<RoleResponse>.Create(items, total, input.PageIndex, input.PageSize);
    }

    /// <summary>
    /// 获取所有角色
    /// </summary>
    public async Task<List<RoleResponse>> GetAllAsync(string? name = null)
    {
        var list = await _db.Queryable<RoleEntity>()
            .WhereIF(name.IsNotNullOrWhiteSpace(), r => r.Name.Contains(name!))
            .OrderByDescending(r => r.CreateTime)
            .ToListAsync();

        return list.Adapt<List<RoleResponse>>();
    }

    /// <summary>
    /// 获取角色详情
    /// </summary>
    public async Task<RoleResponse> GetAsync(long id)
    {
        var entity = await _db.Queryable<RoleEntity>().FirstAsync(r => r.Id == id);
        if (entity == null)
            throw BusinessException.Throw(ErrorTipsEnum.NoResult);

        return entity.Adapt<RoleResponse>();
    }

    /// <summary>
    /// 添加角色
    /// </summary>
    public async Task<long> AddAsync(AddRoleRequest input)
    {
        // 检查角色编码是否存在
        var exists = await _db.Queryable<RoleEntity>()
            .AnyAsync(r => r.Code == input.Code);
        if (exists)
            throw BusinessException.Throw("角色编码已存在");

        var entity = input.Adapt<RoleEntity>();
        return await _db.Insertable(entity).ExecuteReturnSnowflakeIdAsync();
    }

    /// <summary>
    /// 更新角色
    /// </summary>
    public async Task UpdateAsync(long id, AddRoleRequest input)
    {
        var entity = await _db.Queryable<RoleEntity>().FirstAsync(r => r.Id == id);
        if (entity == null)
            throw BusinessException.Throw(ErrorTipsEnum.NoResult);

        // 检查角色编码是否被其他角色使用
        var exists = await _db.Queryable<RoleEntity>()
            .AnyAsync(r => r.Code == input.Code && r.Id != id);
        if (exists)
            throw BusinessException.Throw("角色编码已存在");

        entity = input.Adapt(entity);
        await _db.Updateable(entity)
            .IgnoreColumns(r => new { r.CreateBy, r.CreateTime })
            .ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除角色
    /// </summary>
    public async Task DeleteAsync(long id)
    {
        var entity = await _db.Queryable<RoleEntity>().FirstAsync(r => r.Id == id);
        if (entity == null)
            throw BusinessException.Throw(ErrorTipsEnum.NoResult);

        // 检查是否有用户使用该角色
        var hasUsers = await _db.Queryable<UserRoleEntity>().AnyAsync(ur => ur.RoleId == id);
        if (hasUsers)
            throw BusinessException.Throw("该角色下存在用户，无法删除");

        // 删除角色功能关联
        await _db.Deleteable<RoleFunctionEntity>().Where(rf => rf.RoleId == id).ExecuteCommandAsync();

        // 软删除角色
        entity.IsDeleted = true;
        await _db.Updateable(entity).UpdateColumns(r => r.IsDeleted).ExecuteCommandAsync();

        // 角色变更，清除所有权限和路由缓存（激进但安全）
        _cache.RemoveByPrefix(PermissionCachePrefix);
        _cache.RemoveByPrefix(RouterCachePrefix);
    }

    /// <summary>
    /// 分配功能给角色
    /// </summary>
    public async Task AssignFunctionsAsync(AssignRoleFunctionsRequest input)
    {
        var entity = await _db.Queryable<RoleEntity>().FirstAsync(r => r.Id == input.RoleId);
        if (entity == null)
            throw BusinessException.Throw(ErrorTipsEnum.NoResult);

        // 删除原有关联
        await _db.Deleteable<RoleFunctionEntity>().Where(rf => rf.RoleId == input.RoleId).ExecuteCommandAsync();

        // 添加新关联
        if (input.FunctionIds.Count > 0)
        {
            var entities = input.FunctionIds.Select(fid => new RoleFunctionEntity
            {
                RoleId = input.RoleId,
                FunctionId = fid
            }).ToList();

            await _db.Insertable(entities).ExecuteReturnSnowflakeIdListAsync();
        }

        // 权限变更，清除所有权限和路由缓存
        _cache.RemoveByPrefix(PermissionCachePrefix);
        _cache.RemoveByPrefix(RouterCachePrefix);
    }

    /// <summary>
    /// 获取角色的功能ID列表
    /// </summary>
    public async Task<List<long>> GetFunctionIdsAsync(long roleId)
    {
        var functions = await _db.Queryable<RoleFunctionEntity>()
            .Where(rf => rf.RoleId == roleId)
            .Select(rf => rf.FunctionId)
            .ToListAsync();
        return functions;
    }
}

