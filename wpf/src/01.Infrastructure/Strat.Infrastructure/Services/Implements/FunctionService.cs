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

    public async Task<long> AddAsync(AddFunctionInput input)
    {
        var response = await _functionApi.AddAsync(input);
        return response.Data;
    }

    public async Task<bool> UpdateAsync(UpdateFunctionInput input)
    {
        var response = await _functionApi.UpdateAsync(input);
        return response.Data;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var response = await _functionApi.DeleteAsync(id);
        return response.Data;
    }

    public async Task<bool> BatchDeleteAsync(List<long> ids)
    {
        var response = await _functionApi.BatchDeleteAsync(ids);
        return response.Data;
    }
}
