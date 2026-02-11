using Strat.Infrastructure.Models.Organization;
using Strat.Infrastructure.Services.Abstractions;
using Strat.Infrastructure.Services.Refit;

namespace Strat.Infrastructure.Services.Implements;

public class OrganizationService(IOrganizationApi api) : IOrganizationService
{
    private readonly IOrganizationApi _api = api;

    public async Task<List<OrganizationResponse>> GetTreeAsync()
    {
        var response = await _api.GetTreeAsync();
        return response.Data ?? new List<OrganizationResponse>();
    }

    public async Task<List<OrganizationResponse>> GetListAsync()
    {
        var response = await _api.GetListAsync();
        return response.Data ?? new List<OrganizationResponse>();
    }

    public async Task<bool> AddAsync(AddOrganizationInput input)
    {
        var response = await _api.AddAsync(input);
        return response.Data;
    }

    public async Task<bool> UpdateAsync(UpdateOrganizationInput input)
    {
        var response = await _api.UpdateAsync(input);
        return response.Data;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var response = await _api.DeleteAsync(id);
        return response.Data;
    }
}
