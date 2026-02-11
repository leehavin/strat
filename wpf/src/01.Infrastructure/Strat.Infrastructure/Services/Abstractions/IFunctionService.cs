using Strat.Infrastructure.Models.Function;

namespace Strat.Infrastructure.Services.Abstractions;

public interface IFunctionService
{
    Task<List<FunctionResponse>> GetTreeAsync();
}
