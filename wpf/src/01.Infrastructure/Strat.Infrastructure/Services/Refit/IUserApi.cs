using Refit;
using Strat.Infrastructure.Models.User;
using Strat.Shared.Models;

namespace Strat.Infrastructure.Services.Refit;

public interface IUserApi
{
    [Get("/user/paged-list")]
    Task<Strat.Shared.Models.ApiResponse<PagedResult<UserResponse>>> GetPagedListAsync([Body] GetPagedListRequest input);

    [Post("/user/add")]
    Task<Strat.Shared.Models.ApiResponse<bool>> AddAsync([Body] AddUserInput input);

    [Put("/user/update")]
    Task<Strat.Shared.Models.ApiResponse<bool>> UpdateAsync([Body] UpdateUserInput input);

    [Delete("/api/v1/user/delete/{id}")]
    Task<Strat.Shared.Models.ApiResponse<bool>> DeleteAsync(long id);

    [Post("/user/batch-delete")]
    Task<Strat.Shared.Models.ApiResponse<bool>> BatchDeleteAsync([Body] List<long> ids);

    [Put("/api/v1/user/reset-password")]
    Task<Strat.Shared.Models.ApiResponse<bool>> ResetPasswordAsync([Body] object input);

    [Put("/api/v1/user/change-status")]
    Task<Strat.Shared.Models.ApiResponse<bool>> ChangeStatusAsync([Body] object input);
}
