using Refit;
using Strat.Infrastructure.Models.Role;
using Strat.Shared.Models;

namespace Strat.Infrastructure.Services.Refit;

public interface IRoleApi
{
    [Post("/api/sys/role/page")]
    Task<Strat.Shared.Models.ApiResponse<PagedResult<RoleResponse>>> GetPagedListAsync([Body] GetRolePagedRequest input);

    [Post("/api/sys/role/add")]
    Task<Strat.Shared.Models.ApiResponse<long>> AddAsync([Body] AddRoleInput input);

    [Put("/api/sys/role/update")]
    Task<Strat.Shared.Models.ApiResponse<bool>> UpdateAsync([Body] UpdateRoleInput input);

    [Delete("/api/sys/role/delete")]
    Task<Strat.Shared.Models.ApiResponse<bool>> DeleteAsync(long id);

    [Post("/api/sys/role/batch-delete")]
    Task<Strat.Shared.Models.ApiResponse<bool>> BatchDeleteAsync([Body] List<long> ids);

    [Put("/api/sys/role/change-status")]
    Task<Strat.Shared.Models.ApiResponse<bool>> ChangeStatusAsync([Body] object input);

    [Post("/api/sys/role/assign-functions")]
    Task<Strat.Shared.Models.ApiResponse<bool>> AssignFunctionsAsync([Body] AssignRoleFunctionsRequest input);

    [Get("/api/sys/role/function-ids")]
    Task<Strat.Shared.Models.ApiResponse<List<long>>> GetFunctionIdsAsync(long roleId);
}
