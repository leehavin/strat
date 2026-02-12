using Refit;
using Strat.Infrastructure.Models.Organization;
using Strat.Shared.Models;

namespace Strat.Infrastructure.Services.Refit;

public interface IOrganizationApi
{
    [Get("/organization/tree")]
    Task<Strat.Shared.Models.ApiResponse<List<OrganizationResponse>>> GetTreeAsync();

    [Get("/organization/list")]
    Task<Strat.Shared.Models.ApiResponse<List<OrganizationResponse>>> GetListAsync();

    [Post("/organization/add")]
    Task<Strat.Shared.Models.ApiResponse<bool>> AddAsync([Body] AddOrganizationInput input);

    [Put("/organization/update")]
    Task<Strat.Shared.Models.ApiResponse<bool>> UpdateAsync([Body] UpdateOrganizationInput input);

    [Delete("/organization/delete")]
    Task<Strat.Shared.Models.ApiResponse<bool>> DeleteAsync(long id);
}
