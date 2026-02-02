using Strat.System.Application.Contracts.Organization.Dtos;
using Volo.Abp.Application.Services;

namespace Strat.System.Application.Contracts.Organization;

/// <summary>
/// 组织架构服务接口
/// </summary>
public interface IOrganizationService : IApplicationService
{
    Task<List<OrganizationResponse>> GetTreeAsync();
    Task<OrganizationResponse> GetAsync(long id);
    Task<long> AddAsync(AddOrganizationRequest input);
    Task UpdateAsync(long id, AddOrganizationRequest input);
    Task DeleteAsync(long id);
}

