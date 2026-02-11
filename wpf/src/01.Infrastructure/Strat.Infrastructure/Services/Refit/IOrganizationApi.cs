using Refit;
using Strat.Infrastructure.Models.Organization;
using Strat.Shared.Models;

namespace Strat.Infrastructure.Services.Refit;

public interface IOrganizationApi
{
    [Get("/api/sys/organization/tree")]
    Task<Strat.Shared.Models.ApiResponse<List<OrganizationResponse>>> GetTreeAsync();

    [Get("/api/sys/organization/list")]
    Task<Strat.Shared.Models.ApiResponse<List<OrganizationResponse>>> GetListAsync();

    [Post("/api/sys/organization/add")]
    Task<Strat.Shared.Models.ApiResponse<bool>> AddAsync([Body] AddOrganizationInput input);

    [Put("/api/sys/organization/update")]
    Task<Strat.Shared.Models.ApiResponse<bool>> UpdateAsync([Body] UpdateOrganizationInput input);

    [Delete("/api/sys/organization/delete")]
    Task<Strat.Shared.Models.ApiResponse<bool>> DeleteAsync(long id);
}
