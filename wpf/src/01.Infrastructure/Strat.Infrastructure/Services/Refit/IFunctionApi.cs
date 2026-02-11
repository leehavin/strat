using Refit;
using Strat.Infrastructure.Models.Function;
using Strat.Shared.Models;

namespace Strat.Infrastructure.Services.Refit;

public interface IFunctionApi
{
    [Get("/function/tree")]
    Task<Strat.Shared.Models.ApiResponse<List<FunctionResponse>>> GetTreeAsync();
}
