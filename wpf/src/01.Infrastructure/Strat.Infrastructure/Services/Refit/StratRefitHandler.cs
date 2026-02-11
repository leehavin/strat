using Strat.Shared;
using Strat.Shared.Logging;
using System.Net.Http.Headers;

namespace Strat.Infrastructure.Services.Refit;

public class StratRefitHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Add Bearer Token
        var token = TokenManager.GetToken();
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        StratLogger.Information($"[Refit Request] {request.Method} {request.RequestUri}");

        var response = await base.SendAsync(request, cancellationToken);

        StratLogger.Information($"[Refit Response] {request.Method} {request.RequestUri} - {response.StatusCode}");

        // Here we could add logic to handle token refresh or unauthorized events
        // matching what StratHttpService does.
        
        return response;
    }
}
