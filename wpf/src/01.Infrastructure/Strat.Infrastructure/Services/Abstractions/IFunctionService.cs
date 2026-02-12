using Strat.Infrastructure.Models.Function;

namespace Strat.Infrastructure.Services.Abstractions;

public interface IFunctionService
{
    Task<List<FunctionResponse>> GetTreeAsync();
    Task<long> AddAsync(AddFunctionInput input);
    Task<bool> UpdateAsync(UpdateFunctionInput input);
    Task<bool> DeleteAsync(long id);
    Task<bool> BatchDeleteAsync(List<long> ids);
}
