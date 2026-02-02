using Strat.Shared.Models;
using Strat.Infrastructure.Models.Role;

namespace Strat.Infrastructure.Services.Abstractions
{
    public interface IRoleService
    {
        Task<PagedResult<RoleResponse>> GetPagedListAsync(GetRolePagedRequest input);
    }
}

