using Strat.System.Application.Contracts.Dict.Dtos;
using Volo.Abp.Application.Services;

namespace Strat.System.Application.Contracts.Dict;

/// <summary>
/// 字典分类服务接口
/// </summary>
public interface IDictCategoryService : IApplicationService
{
    Task<PagedList<DictCategoryResponse>> GetPagedListAsync(GetDictPagedRequest input);
    Task<List<DictCategoryResponse>> GetAllAsync();
    Task<DictCategoryResponse> GetAsync(long id);
    Task<long> AddAsync(AddDictCategoryRequest input);
    Task UpdateAsync(long id, AddDictCategoryRequest input);
    Task DeleteAsync(long id);
}

