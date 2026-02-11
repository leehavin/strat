using Strat.Infrastructure.Models.User;
using Strat.Shared.Models;

namespace Strat.Infrastructure.Services.Abstractions;

public interface IUserService
{
    Task<PagedResult<UserResponse>> GetPagedListAsync(GetPagedListRequest input);

    Task<bool> AddAsync(AddUserInput input);

    Task<bool> UpdateAsync(UpdateUserInput input);

    Task<bool> DeleteAsync(long id);
    
    Task<bool> BatchDeleteAsync(List<long> ids);

    Task<bool> ResetPasswordAsync(long id, string newPassword);

    Task<bool> ChangeStatusAsync(long id, int status);
}
