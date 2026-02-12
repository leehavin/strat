using Strat.Infrastructure.Persistence;

namespace Strat.Identity.Application.Function;

/// <summary>
/// 功能服务
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroupConst.System)]
[Authorize]
public class FunctionService(
    ISqlSugarClient db,
    IRepository<FunctionEntity> functionRepository,
    ICache cache) : ApplicationService, IFunctionService
{
    private readonly ISqlSugarClient _db = db;
    private readonly IRepository<FunctionEntity> _functionRepository = functionRepository;
    private readonly ICache _cache = cache;

    /// <summary>
    /// 分页查询
    /// </summary>
    public async Task<PagedList<FunctionResponse>> GetPagedListAsync(GetFunctionPagedRequest input)
    {
        List<FunctionEntity> list;

        if (string.IsNullOrWhiteSpace(input.Name))
        {
            // 无搜索条件，返回树形结构
            list = await _functionRepository.Queryable()
                .OrderBy(x => x.Sort)
                .ToTreeAsync(x => x.Children, x => x.ParentId, null);
        }
        else
        {
            // 有搜索条件，返回平铺列表
            list = await _functionRepository.Queryable()
                .Where(x => x.Name.Contains(input.Name))
                .OrderBy(x => x.Sort)
                .ToListAsync();
        }

        return list.Adapt<List<FunctionResponse>>().ToPurestPagedList(input.PageIndex, input.PageSize);
    }

    /// <summary>
    /// 获取单条记录
    /// </summary>
    public async Task<FunctionResponse> GetAsync(long id)
    {
        var entity = await _functionRepository.GetByIdAsync(id)
            ?? throw BusinessException.Throw(ErrorTipsEnum.NoResult);
        return entity.Adapt<FunctionResponse>();
    }

    /// <summary>
    /// 获取功能树
    /// </summary>
    public async Task<List<FunctionResponse>> GetTreeAsync()
    {
        var tree = await _functionRepository.Queryable()
            .OrderBy(x => x.Sort)
            .ToTreeAsync(x => x.Children, x => x.ParentId, null);
        return tree.Adapt<List<FunctionResponse>>();
    }

    /// <summary>
    /// 添加功能
    /// </summary>
    public async Task<long> AddAsync(AddFunctionRequest input)
    {
        // 检查编码是否重复
        var exists = await _functionRepository.Queryable()
            .AnyAsync(x => x.Code == input.Code);
        if (exists)
        {
            throw BusinessException.Throw("功能编码已存在");
        }

        var entity = input.Adapt<FunctionEntity>();
        return await _functionRepository.InsertReturnSnowflakeIdAsync(entity);
    }

    /// <summary>
    /// 更新功能
    /// </summary>
    public async Task UpdateAsync(long id, AddFunctionRequest input)
    {
        var entity = await _functionRepository.GetByIdAsync(id)
            ?? throw BusinessException.Throw(ErrorTipsEnum.NoResult);

        // 检查编码是否重复（排除自身）
        var exists = await _functionRepository.Queryable()
            .AnyAsync(x => x.Code == input.Code && x.Id != id);
        if (exists)
        {
            throw BusinessException.Throw("功能编码已存在");
        }

        input.Adapt(entity);
        await _functionRepository.UpdateAsync(entity);

        // 清除权限和路由缓存
        _cache.RemoveByPrefix("Auth:Permissions:");
        _cache.RemoveByPrefix("Auth:Routers:");
    }

    /// <summary>
    /// 删除功能
    /// </summary>
    public async Task DeleteAsync(long id)
    {
        var entity = await _functionRepository.GetByIdAsync(id)
            ?? throw BusinessException.Throw(ErrorTipsEnum.NoResult);

        // 检查是否有子功能
        var hasChildren = await _functionRepository.Queryable()
            .AnyAsync(x => x.ParentId == id);
        if (hasChildren)
        {
            throw BusinessException.Throw("存在子功能，无法删除");
        }

        await _functionRepository.DeleteAsync(entity);

        // 清除相关缓存
        _cache.RemoveByPrefix(CacheConst.RolesInterfacePrefix);
        _cache.RemoveByPrefix("Auth:Permissions:");
        _cache.RemoveByPrefix("Auth:Routers:");
    }

    /// <summary>
    /// 获取功能绑定的接口
    /// </summary>
    public async Task<List<BindedInterfaceResponse>> GetInterfacesAsync(long functionId)
    {
        var interfaces = await _db.Queryable<FunctionInterfaceEntity>()
            .LeftJoin<InterfaceQueryEntity>((fi, i) => fi.InterfaceId == i.Id)
            .Where((fi, i) => fi.FunctionId == functionId)
            .Select((fi, i) => new BindedInterfaceResponse
            {
                Id = fi.Id,
                InterfaceId = fi.InterfaceId,
                Name = i.Name,
                Path = i.Path
            })
            .ToListAsync();
        return interfaces;
    }

    /// <summary>
    /// 分配/取消分配接口（切换模式）
    /// </summary>
    public async Task AssignInterfaceAsync(AssignInterfaceRequest input)
    {
        var record = await _db.Queryable<FunctionInterfaceEntity>()
            .FirstAsync(x => x.FunctionId == input.FunctionId && x.InterfaceId == input.InterfaceId);

        if (record == null)
        {
            // 不存在则添加
            await _db.Insertable(new FunctionInterfaceEntity
            {
                FunctionId = input.FunctionId,
                InterfaceId = input.InterfaceId
            }).ExecuteReturnSnowflakeIdAsync();
        }
        else
        {
            // 存在则删除
            await _db.Deleteable(record).ExecuteCommandAsync();
        }

        // 清除缓存
        _cache.RemoveByPrefix(CacheConst.RolesInterfacePrefix);
    }

    /// <summary>
    /// 删除功能接口绑定
    /// </summary>
    public async Task DeleteFunctionInterfaceAsync(long id)
    {
        var record = await _db.Queryable<FunctionInterfaceEntity>()
            .FirstAsync(x => x.Id == id)
            ?? throw BusinessException.Throw(ErrorTipsEnum.NoResult);

        await _db.Deleteable(record).ExecuteCommandAsync();

        // 清除缓存
        _cache.RemoveByPrefix(CacheConst.RolesInterfacePrefix);
    }
}

/// <summary>
/// 接口查询实体（跨模块查询用）
/// </summary>
[SugarTable("INTERFACE")]
internal class InterfaceQueryEntity
{
    [SugarColumn(ColumnName = "ID", IsPrimaryKey = true)]
    public long Id { get; set; }

    [SugarColumn(ColumnName = "NAME")]
    public string Name { get; set; } = string.Empty;

    [SugarColumn(ColumnName = "PATH")]
    public string Path { get; set; } = string.Empty;
}

