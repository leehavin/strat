using Strat.Shared;
using Strat.Shared.Logging;
using Strat.Shared.Events;
using Prism.Events;
using System.Net.Http.Headers;

namespace Strat.Infrastructure.Services.Refit;

public class StratRefitHandler : DelegatingHandler
{
    private readonly IEventAggregator _eventAggregator;

    public StratRefitHandler(IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Add Bearer Token
        var token = TokenManager.GetToken();
        if (!string.IsNullOrEmpty(token))
        {
            // 确保没有引号
            token = token.Trim('"');
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            StratLogger.Information($"[Refit Request] {request.Method} {request.RequestUri} (With Token: {token.Substring(0, Math.Min(10, token.Length))}...)");
        }
        else
        {
            StratLogger.Information($"[Refit Request] {request.Method} {request.RequestUri} (No Token)");
        }

        var response = await base.SendAsync(request, cancellationToken);

        StratLogger.Information($"[Refit Response] {request.Method} {request.RequestUri} - {response.StatusCode}");

        // 尝试从多个可能的头中提取 Token (不区分大小写)
        string? newToken = null;
        
        // 1. 检查 Headers
        if (response.Headers.TryGetValues("accesstoken", out var values))
        {
            newToken = values.FirstOrDefault();
        }
        
        // 2. 如果没找到，检查 Content Headers
        if (string.IsNullOrEmpty(newToken) && response.Content != null && response.Content.Headers.TryGetValues("accesstoken", out var contentValues))
        {
            newToken = contentValues.FirstOrDefault();
        }

        if (!string.IsNullOrEmpty(newToken))
        {
            // 清理可能存在的引号
            newToken = newToken.Trim('"');
            TokenManager.SaveToken(newToken);
            StratLogger.Information($"[Refit] 成功从响应头提取并保存新 Token: {newToken.Substring(0, Math.Min(10, newToken.Length))}...");
        }

        // 处理 401 未授权
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            StratLogger.Warning($"[Refit] 收到 401 未授权响应: {request.RequestUri}");
            TokenManager.ClearToken();
            
            // 发布未授权事件，触发跳转到登录页
            _eventAggregator.GetEvent<UnauthorizedEvent>().Publish();
        }
        
        return response;
    }
}
