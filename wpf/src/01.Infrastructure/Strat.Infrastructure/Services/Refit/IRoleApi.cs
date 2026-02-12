using Refit;
using Strat.Infrastructure.Models.Role;
using Strat.Shared.Models;

namespace Strat.Infrastructure.Services.Refit;

public interface IRoleApi
{
    [Get("/role/paged-list")]
    Task<Strat.Shared.Models.ApiResponse<PagedResult<RoleResponse>>> GetPagedListAsync([Body] GetRolePagedRequest input);

    [Post("/role/add")]
    Task<Strat.Shared.Models.ApiResponse<long>> AddAsync([Body] AddRoleInput input);

    [Put("/role/update")]
    Task<Strat.Shared.Models.ApiResponse<bool>> UpdateAsync([Body] UpdateRoleInput input);

    [Delete("/role/delete")]
    Task<Strat.Shared.Models.ApiResponse<bool>> DeleteAsync(long id);

    [Post("/role/batch-delete")]
    Task<Strat.Shared.Models.ApiResponse<bool>> BatchDeleteAsync([Body] List<long> ids);

    [Put("/role/change-status")]
    Task<Strat.Shared.Models.ApiResponse<bool>> ChangeStatusAsync([Body] object input);

    [Post("/role/assign-functions")]
    Task<Strat.Shared.Models.ApiResponse<bool>> AssignFunctionsAsync([Body] AssignRoleFunctionsRequest input);

    [Get("/role/function-ids/{roleId}")]
    Task<Strat.Shared.Models.ApiResponse<List<long>>> GetFunctionIdsAsync(long roleId);
}
