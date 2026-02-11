using Strat.Infrastructure.Models.Role;
using Strat.Infrastructure.Services.Abstractions;
using Strat.Infrastructure.Services.Refit;
using Strat.Shared.Models;

namespace Strat.Infrastructure.Services.Implements;

public class RoleService(IRoleApi roleApi) : IRoleService
{
    private readonly IRoleApi _roleApi = roleApi;

    public async Task<PagedResult<RoleResponse>> GetPagedListAsync(GetRolePagedRequest input)
    {
        var response = await _roleApi.GetPagedListAsync(input);
        return response.Data!;
    }

    public async Task<bool> AddAsync(AddRoleInput input)
    {
        var response = await _roleApi.AddAsync(input);
        return response.Data;
    }

    public async Task<bool> UpdateAsync(UpdateRoleInput input)
    {
        var response = await _roleApi.UpdateAsync(input);
        return response.Data;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var response = await _roleApi.DeleteAsync(id);
        return response.Data;
    }

    public async Task<bool> BatchDeleteAsync(List<long> ids)
    {
        var response = await _roleApi.BatchDeleteAsync(ids);
        return response.Data;
    }

    public async Task<bool> ChangeStatusAsync(long id, int status)
    {
        var response = await _roleApi.ChangeStatusAsync(new { Id = id, Status = status });
        return response.Data;
    }
}

