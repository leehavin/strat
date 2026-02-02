using Strat.Shared.CommonRequest;
using Strat.Infrastructure.Models.User;
using Strat.Shared.Models;

namespace Strat.Infrastructure.Services.Abstractions;

public interface IUserService
{
    Task<PagedResult<UserResponse>> GetPagedListAsync(GetPagedListRequest input);
}
