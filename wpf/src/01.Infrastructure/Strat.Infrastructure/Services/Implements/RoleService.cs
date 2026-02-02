using Strat.Shared.Models;
using Strat.Infrastructure.Models.Role;
using Strat.Infrastructure.Services.Abstractions;
using Strat.Shared.HttpService;

namespace Strat.Infrastructure.Services.Implements
{
    public class RoleService(IStratHttpService httpService) : IRoleService
    {
        private readonly IStratHttpService _httpService = httpService;

        public async Task<PagedResult<RoleResponse>> GetPagedListAsync(GetRolePagedRequest input)
        {
            return await _httpService.GetAsync<PagedResult<RoleResponse>>("/role", input);
        }
    }
}

