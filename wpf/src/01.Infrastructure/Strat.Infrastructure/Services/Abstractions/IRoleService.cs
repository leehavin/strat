using Strat.Infrastructure.Models.Role;
using Strat.Shared.Models;

namespace Strat.Infrastructure.Services.Abstractions
{
    public interface IRoleService
    {
        Task<PagedResult<RoleResponse>> GetPagedListAsync(GetRolePagedRequest input);
        Task<bool> AddAsync(AddRoleInput input);
        Task<bool> UpdateAsync(UpdateRoleInput input);
        Task<bool> DeleteAsync(long id);
        Task<bool> BatchDeleteAsync(List<long> ids);
        Task<bool> ChangeStatusAsync(long id, int status);
    }
}

