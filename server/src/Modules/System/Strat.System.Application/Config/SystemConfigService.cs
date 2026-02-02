namespace Strat.System.Application.Config;

/// <summary>
/// 系统配置服务
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroupConst.System)]
[Authorize]
public class SystemConfigService(ISqlSugarClient db) : ApplicationService, ISystemConfigService
{
    private readonly ISqlSugarClient _db = db;

    public async Task<PagedList<SystemConfigResponse>> GetPagedListAsync(PagedRequest input)
    {
        RefAsync<int> total = 0;
        var list = await _db.Queryable<SystemConfigEntity>()
            .OrderByDescending(x => x.CreateTime)
            .ToPageListAsync(input.PageIndex, input.PageSize, total);

        var items = list.Adapt<List<SystemConfigResponse>>();
        return PagedList<SystemConfigResponse>.Create(items, total, input.PageIndex, input.PageSize);
    }

    public async Task<SystemConfigResponse> GetAsync(long id)
    {
        var entity = await _db.Queryable<SystemConfigEntity>().FirstAsync(x => x.Id == id);
        if (entity == null)
            throw BusinessException.Throw(ErrorTipsEnum.NoResult);

        return entity.Adapt<SystemConfigResponse>();
    }

    public async Task<string?> GetValueByCodeAsync(string code)
    {
        var entity = await _db.Queryable<SystemConfigEntity>().FirstAsync(x => x.ConfigCode == code);
        return entity?.ConfigValue;
    }

    public async Task<long> AddAsync(AddSystemConfigRequest input)
    {
        var exists = await _db.Queryable<SystemConfigEntity>().AnyAsync(x => x.ConfigCode == input.ConfigCode);
        if (exists)
            throw BusinessException.Throw("配置编码已存在");

        var entity = input.Adapt<SystemConfigEntity>();
        return await _db.Insertable(entity).ExecuteReturnSnowflakeIdAsync();
    }

    public async Task UpdateAsync(long id, AddSystemConfigRequest input)
    {
        var entity = await _db.Queryable<SystemConfigEntity>().FirstAsync(x => x.Id == id);
        if (entity == null)
            throw BusinessException.Throw(ErrorTipsEnum.NoResult);

        entity = input.Adapt(entity);
        await _db.Updateable(entity).IgnoreColumns(x => new { x.CreateBy, x.CreateTime }).ExecuteCommandAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var entity = await _db.Queryable<SystemConfigEntity>().FirstAsync(x => x.Id == id);
        if (entity == null)
            throw BusinessException.Throw(ErrorTipsEnum.NoResult);

        entity.IsDeleted = true;
        await _db.Updateable(entity).UpdateColumns(x => x.IsDeleted).ExecuteCommandAsync();
    }
}

