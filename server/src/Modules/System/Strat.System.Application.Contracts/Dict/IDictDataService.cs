using Strat.System.Application.Contracts.Dict.Dtos;
using Volo.Abp.Application.Services;

namespace Strat.System.Application.Contracts.Dict;

/// <summary>
/// 字典数据服务接口
/// </summary>
public interface IDictDataService : IApplicationService
{
    Task<PagedList<DictDataResponse>> GetPagedListAsync(GetDictPagedRequest input);
    Task<List<DictDataResponse>> GetByCategoryCodeAsync(string categoryCode);
    Task<DictDataResponse> GetAsync(long id);
    Task<long> AddAsync(AddDictDataRequest input);
    Task UpdateAsync(long id, AddDictDataRequest input);
    Task DeleteAsync(long id);
}

