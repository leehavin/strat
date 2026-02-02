using Strat.System.Application.Contracts.Dict;
using Strat.System.Application.Contracts.Dict.Dtos;
using Strat.System.Domain.Dict;

namespace Strat.System.Application.Dict;

/// <summary>
/// 字典分类服务
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroupConst.System)]
[Authorize]
public class DictCategoryService(ISqlSugarClient db) : ApplicationService, IDictCategoryService
{
    private readonly ISqlSugarClient _db = db;

    public async Task<PagedList<DictCategoryResponse>> GetPagedListAsync(GetDictPagedRequest input)
    {
        RefAsync<int> total = 0;
        var list = await _db.Queryable<DictCategoryEntity>()
            .WhereIF(input.Name.IsNotNullOrWhiteSpace(), x => x.Name.Contains(input.Name!))
            .OrderByDescending(x => x.CreateTime)
            .ToPageListAsync(input.PageIndex, input.PageSize, total);

        var items = list.Adapt<List<DictCategoryResponse>>();
        return PagedList<DictCategoryResponse>.Create(items, total, input.PageIndex, input.PageSize);
    }

    public async Task<List<DictCategoryResponse>> GetAllAsync()
    {
        var list = await _db.Queryable<DictCategoryEntity>()
            .OrderByDescending(x => x.CreateTime)
            .ToListAsync();
        return list.Adapt<List<DictCategoryResponse>>();
    }

    public async Task<DictCategoryResponse> GetAsync(long id)
    {
        var entity = await _db.Queryable<DictCategoryEntity>().FirstAsync(x => x.Id == id);
        if (entity == null)
            throw BusinessException.Throw(ErrorTipsEnum.NoResult);
        return entity.Adapt<DictCategoryResponse>();
    }

    public async Task<long> AddAsync(AddDictCategoryRequest input)
    {
        var exists = await _db.Queryable<DictCategoryEntity>().AnyAsync(x => x.Code == input.Code);
        if (exists)
            throw BusinessException.Throw("分类编码已存在");

        var entity = input.Adapt<DictCategoryEntity>();
        return await _db.Insertable(entity).ExecuteReturnSnowflakeIdAsync();
    }

    public async Task UpdateAsync(long id, AddDictCategoryRequest input)
    {
        var entity = await _db.Queryable<DictCategoryEntity>().FirstAsync(x => x.Id == id);
        if (entity == null)
            throw BusinessException.Throw(ErrorTipsEnum.NoResult);

        var exists = await _db.Queryable<DictCategoryEntity>().AnyAsync(x => x.Code == input.Code && x.Id != id);
        if (exists)
            throw BusinessException.Throw("分类编码已存在");

        entity = input.Adapt(entity);
        await _db.Updateable(entity).IgnoreColumns(x => new { x.CreateBy, x.CreateTime }).ExecuteCommandAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var entity = await _db.Queryable<DictCategoryEntity>().FirstAsync(x => x.Id == id);
        if (entity == null)
            throw BusinessException.Throw(ErrorTipsEnum.NoResult);

        var hasData = await _db.Queryable<DictDataEntity>().AnyAsync(x => x.CategoryId == id);
        if (hasData)
            throw BusinessException.Throw("该分类下存在字典数据，无法删除");

        entity.IsDeleted = true;
        await _db.Updateable(entity).UpdateColumns(x => x.IsDeleted).ExecuteCommandAsync();
    }
}

