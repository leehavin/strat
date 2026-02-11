using Strat.Infrastructure.Models.User;
using Strat.Infrastructure.Services.Abstractions;
using Strat.Infrastructure.Services.Refit;
using Strat.Shared.Models;

namespace Strat.Infrastructure.Services.Implements;

public class UserService(IUserApi userApi) : IUserService
{
    private readonly IUserApi _userApi = userApi;

    public async Task<PagedResult<UserResponse>> GetPagedListAsync(GetPagedListRequest input)
    {
        var response = await _userApi.GetPagedListAsync(input);
        return response.Data!;
    }

    public async Task<bool> AddAsync(AddUserInput input)
    {
        var response = await _userApi.AddAsync(input);
        return response.Data;
    }

    public async Task<bool> UpdateAsync(UpdateUserInput input)
    {
        var response = await _userApi.UpdateAsync(input);
        return response.Data;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var response = await _userApi.DeleteAsync(id);
        return response.Data;
    }

    public async Task<bool> BatchDeleteAsync(List<long> ids)
    {
        var response = await _userApi.BatchDeleteAsync(ids);
        return response.Data;
    }

    public async Task<bool> ResetPasswordAsync(long id, string newPassword)
    {
        var response = await _userApi.ResetPasswordAsync(new { Id = id, Password = newPassword });
        return response.Data;
    }

    public async Task<bool> ChangeStatusAsync(long id, int status)
    {
        var response = await _userApi.ChangeStatusAsync(new { Id = id, Status = status });
        return response.Data;
    }
}

