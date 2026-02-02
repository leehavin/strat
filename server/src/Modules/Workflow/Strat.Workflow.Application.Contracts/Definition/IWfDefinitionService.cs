using Strat.Workflow.Application.Contracts.Definition.Dtos;
using Volo.Abp.Application.Services;

namespace Strat.Workflow.Application.Contracts.Definition;

public interface IWfDefinitionService : IApplicationService
{
    Task<PagedList<WfDefinitionResponse>> GetPagedListAsync(PagedRequest input);
    Task<WfDefinitionResponse> GetAsync(long id);
    Task<long> AddAsync(AddWfDefinitionRequest input);
    Task UpdateAsync(long id, AddWfDefinitionRequest input);
    Task DeleteAsync(long id);
    Task LockAsync(long id);
    Task UnlockAsync(long id);
}

