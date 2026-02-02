using Strat.Infrastructure.Models.User;
using Strat.Infrastructure.Services.Abstractions;
using Strat.Shared.HttpService;
using Strat.Shared.Models;

namespace Strat.Infrastructure.Services.Implements;

public class UserService(IStratHttpService httpService) : IUserService
{
    private readonly IStratHttpService _httpService = httpService;

    public async Task<PagedResult<UserResponse>> GetPagedListAsync(GetPagedListRequest input)
    {
        return await _httpService.GetAsync<PagedResult<UserResponse>>("/user/paged-list", input);
    }
}

