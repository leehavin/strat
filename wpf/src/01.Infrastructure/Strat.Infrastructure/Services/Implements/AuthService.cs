using Strat.Infrastructure.Models.Auth;
using Strat.Infrastructure.Services.Abstractions;
using Strat.Infrastructure.Services.Refit;

namespace Strat.Infrastructure.Services.Implements;

public class AuthService(IAuthApi authApi) : IAuthService
{
    private readonly IAuthApi _authApi = authApi;

    public async Task<LoginResponse> LoginAsync(LoginRequest input)
    {
        var response = await _authApi.LoginAsync(input);
        return response.Data!;
    }

    public async Task<List<GetRoutersResponse>> GetRoutersAsync()
    {
        var response = await _authApi.GetRoutersAsync();
        return response.Data!;
    }

    public async Task<GetUserInfoResponse> GetUserInfoAsync()
    {
        var response = await _authApi.GetUserInfoAsync();
        return response.Data!;
    }
}

