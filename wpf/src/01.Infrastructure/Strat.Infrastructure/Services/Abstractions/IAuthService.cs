using Strat.Infrastructure.Models.Auth;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Strat.Infrastructure.Services.Abstractions
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginRequest input);
        Task<List<GetRoutersResponse>> GetRoutersAsync();
        Task<GetUserInfoResponse> GetUserInfoAsync();
    }
}
