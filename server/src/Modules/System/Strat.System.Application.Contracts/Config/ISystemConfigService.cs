using Strat.System.Application.Contracts.Config.Dtos;
using Volo.Abp.Application.Services;

namespace Strat.System.Application.Contracts.Config;

public interface ISystemConfigService : IApplicationService
{
    Task<PagedList<SystemConfigResponse>> GetPagedListAsync(PagedRequest input);
    Task<SystemConfigResponse> GetAsync(long id);
    Task<string?> GetValueByCodeAsync(string code);
    Task<long> AddAsync(AddSystemConfigRequest input);
    Task UpdateAsync(long id, AddSystemConfigRequest input);
    Task DeleteAsync(long id);
}

