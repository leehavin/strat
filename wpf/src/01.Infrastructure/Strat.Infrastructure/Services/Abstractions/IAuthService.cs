using Strat.Infrastructure.Models.Auth;

namespace Strat.Infrastructure.Services.Abstractions
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginRequest input);
        Task<List<GetRoutersResponse>> GetRoutersAsync();
        Task<GetUserInfoResponse> GetUserInfoAsync();
        Task LogoutAsync();
    }
}
