using Strat.System.Application.Contracts.Dict;
using Strat.System.Application.Contracts.Dict.Dtos;
using Strat.System.Domain.Dict;

namespace Strat.System.Application.Dict;

/// <summary>
/// 字典数据服务
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroupConst.System)]
[Authorize]
public class DictDataService(ISqlSugarClient db) : ApplicationService, IDictDataService
{
    private readonly ISqlSugarClient _db = db;

    public async Task<PagedList<DictDataResponse>> GetPagedListAsync(GetDictPagedRequest input)
    {
        RefAsync<int> total = 0;
        var list = await _db.Queryable<DictDataEntity>()
            .LeftJoin<DictCategoryEntity>((d, c) => d.CategoryId == c.Id)
            .WhereIF(input.Name.IsNotNullOrWhiteSpace(), (d, c) => d.Name.Contains(input.Name!))
            .WhereIF(input.CategoryId.HasValue, (d, c) => d.CategoryId == input.CategoryId)
            .OrderBy((d, c) => d.Sort)
            .OrderByDescending((d, c) => d.CreateTime)
            .Select((d, c) => new DictDataEntity
            {
                Id = d.Id,
                CategoryId = d.CategoryId,
                CategoryName = c.Name,
                Name = d.Name,
                Code = d.Code,
                Sort = d.Sort,
                Remark = d.Remark,
                CreateTime = d.CreateTime
            })
            .ToPageListAsync(input.PageIndex, input.PageSize, total);

        var items = list.Adapt<List<DictDataResponse>>();
        return PagedList<DictDataResponse>.Create(items, total, input.PageIndex, input.PageSize);
    }

    public async Task<List<DictDataResponse>> GetByCategoryCodeAsync(string categoryCode)
    {
        var list = await _db.Queryable<DictDataEntity>()
            .InnerJoin<DictCategoryEntity>((d, c) => d.CategoryId == c.Id)
            .Where((d, c) => c.Code == categoryCode)
            .OrderBy((d, c) => d.Sort)
            .Select((d, c) => d)
            .ToListAsync();
        return list.Adapt<List<DictDataResponse>>();
    }

    public async Task<DictDataResponse> GetAsync(long id)
    {
        var entity = await _db.Queryable<DictDataEntity>().FirstAsync(x => x.Id == id);
        if (entity == null)
            throw BusinessException.Throw(ErrorTipsEnum.NoResult);
        return entity.Adapt<DictDataResponse>();
    }

    public async Task<long> AddAsync(AddDictDataRequest input)
    {
        var entity = input.Adapt<DictDataEntity>();
        return await _db.Insertable(entity).ExecuteReturnSnowflakeIdAsync();
    }

    public async Task UpdateAsync(long id, AddDictDataRequest input)
    {
        var entity = await _db.Queryable<DictDataEntity>().FirstAsync(x => x.Id == id);
        if (entity == null)
            throw BusinessException.Throw(ErrorTipsEnum.NoResult);

        entity = input.Adapt(entity);
        await _db.Updateable(entity).IgnoreColumns(x => new { x.CreateBy, x.CreateTime }).ExecuteCommandAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var entity = await _db.Queryable<DictDataEntity>().FirstAsync(x => x.Id == id);
        if (entity == null)
            throw BusinessException.Throw(ErrorTipsEnum.NoResult);

        entity.IsDeleted = true;
        await _db.Updateable(entity).UpdateColumns(x => x.IsDeleted).ExecuteCommandAsync();
    }
}

