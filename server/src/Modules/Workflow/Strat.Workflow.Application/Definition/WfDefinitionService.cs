namespace Strat.Workflow.Application.Definition;

/// <summary>
/// 流程定义服务
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroupConst.Workflow)]
public class WfDefinitionService(ISqlSugarClient db) : ApplicationService, IWfDefinitionService
{
    private readonly ISqlSugarClient _db = db;

    public async Task<PagedList<WfDefinitionResponse>> GetPagedListAsync(PagedRequest input)
    {
        RefAsync<int> total = 0;
        var list = await _db.Queryable<WfDefinitionEntity>()
            .Where(x => !x.IsDeleted)
            .OrderByDescending(x => x.CreateTime)
            .ToPageListAsync(input.PageIndex, input.PageSize, total);

        var items = list.Adapt<List<WfDefinitionResponse>>();
        return PagedList<WfDefinitionResponse>.Create(items, total, input.PageIndex, input.PageSize);
    }

    public async Task<WfDefinitionResponse> GetAsync(long id)
    {
        var entity = await _db.Queryable<WfDefinitionEntity>().FirstAsync(x => x.Id == id && !x.IsDeleted);
        if (entity == null)
            throw BusinessException.Throw(ErrorTipsEnum.NoResult);

        return entity.Adapt<WfDefinitionResponse>();
    }

    public async Task<long> AddAsync(AddWfDefinitionRequest input)
    {
        var entity = input.Adapt<WfDefinitionEntity>();
        entity.DefinitionId = Guid.NewGuid().ToString("N");
        entity.Version = 1;

        return await _db.Insertable(entity).ExecuteReturnSnowflakeIdAsync();
    }

    public async Task UpdateAsync(long id, AddWfDefinitionRequest input)
    {
        var entity = await _db.Queryable<WfDefinitionEntity>().FirstAsync(x => x.Id == id && !x.IsDeleted);
        if (entity == null)
            throw BusinessException.Throw(ErrorTipsEnum.NoResult);

        if (entity.IsLocked)
            throw BusinessException.Throw("流程已锁定，无法修改");

        entity = input.Adapt(entity);
        entity.Version++;

        await _db.Updateable(entity).IgnoreColumns(x => new { x.DefinitionId, x.CreateBy, x.CreateTime }).ExecuteCommandAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var entity = await _db.Queryable<WfDefinitionEntity>().FirstAsync(x => x.Id == id && !x.IsDeleted);
        if (entity == null)
            throw BusinessException.Throw(ErrorTipsEnum.NoResult);

        if (entity.IsLocked)
            throw BusinessException.Throw("流程已锁定，无法删除");

        entity.IsDeleted = true;
        await _db.Updateable(entity).UpdateColumns(x => x.IsDeleted).ExecuteCommandAsync();
    }

    public async Task LockAsync(long id)
    {
        var entity = await _db.Queryable<WfDefinitionEntity>().FirstAsync(x => x.Id == id && !x.IsDeleted);
        if (entity == null)
            throw BusinessException.Throw(ErrorTipsEnum.NoResult);

        entity.IsLocked = true;
        await _db.Updateable(entity).UpdateColumns(x => x.IsLocked).ExecuteCommandAsync();
    }

    public async Task UnlockAsync(long id)
    {
        var entity = await _db.Queryable<WfDefinitionEntity>().FirstAsync(x => x.Id == id && !x.IsDeleted);
        if (entity == null)
            throw BusinessException.Throw(ErrorTipsEnum.NoResult);

        entity.IsLocked = false;
        await _db.Updateable(entity).UpdateColumns(x => x.IsLocked).ExecuteCommandAsync();
    }
}

