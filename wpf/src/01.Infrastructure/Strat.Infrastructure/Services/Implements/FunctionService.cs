using Strat.Infrastructure.Models.Function;
using Strat.Infrastructure.Services.Abstractions;
using Strat.Infrastructure.Services.Refit;

namespace Strat.Infrastructure.Services.Implements;

public class FunctionService(IFunctionApi functionApi) : IFunctionService
{
    private readonly IFunctionApi _functionApi = functionApi;

    public async Task<List<FunctionResponse>> GetTreeAsync()
    {
        var response = await _functionApi.GetTreeAsync();
        return response.Data!;
    }
}
