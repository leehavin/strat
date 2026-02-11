using Refit;
using Strat.Infrastructure.Models.Auth;

namespace Strat.Infrastructure.Services.Refit;

public interface IAuthApi
{
    [Post("/auth/login")]
    Task<Strat.Shared.Models.ApiResponse<LoginResponse>> LoginAsync([Body] LoginRequest input);

    [Get("/auth/routers")]
    Task<Strat.Shared.Models.ApiResponse<List<GetRoutersResponse>>> GetRoutersAsync();

    [Get("/auth/user-info")]
    Task<Strat.Shared.Models.ApiResponse<GetUserInfoResponse>> GetUserInfoAsync();
}
