
using Strat.Infrastructure.Models.Auth;
using Strat.Infrastructure.Services.Abstractions;
using Strat.Shared.HttpService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Strat.Infrastructure.Services.Implements;

public class AuthService(IStratHttpService httpService) : IAuthService
{
    private readonly IStratHttpService _httpService = httpService;

    public async Task<LoginResponse> LoginAsync(LoginRequest input)
    {
        return await _httpService.PostAsync<LoginResponse>("/auth/login", input);
    }

    public async Task<List<GetRoutersResponse>> GetRoutersAsync()
    {
        return await _httpService.GetAsync<List<GetRoutersResponse>>("/auth/routers");
    }

    public async Task<GetUserInfoResponse> GetUserInfoAsync()
    {
        return await _httpService.GetAsync<GetUserInfoResponse>("/auth/user-info");
    }
}

