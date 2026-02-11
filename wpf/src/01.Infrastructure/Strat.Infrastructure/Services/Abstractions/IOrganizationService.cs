using Strat.Infrastructure.Models.Organization;

namespace Strat.Infrastructure.Services.Abstractions;

public interface IOrganizationService
{
    Task<List<OrganizationResponse>> GetTreeAsync();
    Task<List<OrganizationResponse>> GetListAsync();
    Task<bool> AddAsync(AddOrganizationInput input);
    Task<bool> UpdateAsync(UpdateOrganizationInput input);
    Task<bool> DeleteAsync(long id);
}
