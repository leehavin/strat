using Strat.Workflow.Application.Contracts.Instance.Dtos;
using Volo.Abp.Application.Services;

namespace Strat.Workflow.Application.Contracts.Instance;

public interface IWfInstanceService : IApplicationService
{
    Task<PagedList<WfInstanceResponse>> GetMyInstancesAsync(GetWfInstancePagedRequest input);
    Task<PagedList<WfInstanceResponse>> GetPendingTasksAsync(GetWfInstancePagedRequest input);
    Task<WfInstanceResponse> GetAsync(long id);
    Task TerminateAsync(long id);
}

