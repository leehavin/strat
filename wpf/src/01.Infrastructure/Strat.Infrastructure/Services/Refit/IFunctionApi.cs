using Refit;
using Strat.Infrastructure.Models.Function;
using Strat.Shared.Models;

namespace Strat.Infrastructure.Services.Refit;

public interface IFunctionApi
{
    [Get("/function/tree")]
    Task<Strat.Shared.Models.ApiResponse<List<FunctionResponse>>> GetTreeAsync();

    [Post("/function/add")]
    Task<Strat.Shared.Models.ApiResponse<long>> AddAsync([Body] AddFunctionInput input);

    [Put("/function/update")]
    Task<Strat.Shared.Models.ApiResponse<bool>> UpdateAsync([Body] UpdateFunctionInput input);

    [Delete("/function/delete")]
    Task<Strat.Shared.Models.ApiResponse<bool>> DeleteAsync(long id);

    [Post("/function/batch-delete")]
    Task<Strat.Shared.Models.ApiResponse<bool>> BatchDeleteAsync([Body] List<long> ids);
}
