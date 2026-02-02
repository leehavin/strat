using Flurl.Http;
using Flurl.Http.Configuration;
using Strat.Shared.Events;
using Strat.Shared.Logging;
using Strat.Shared.Models;
using System.Net;

namespace Strat.Shared.HttpService
{
    public class StratHttpService : IStratHttpService
    {
        private readonly IFlurlClient _flurlClient;
        private readonly IEventAggregator _eventAggregator;
        
        public StratHttpService(IFlurlClientCache clients, IEventAggregator eventAggregator)
        {
            _flurlClient = clients.Get("strat");
            _eventAggregator = eventAggregator;
            
            _flurlClient.AfterCall(async (call) =>
            {
                _eventAggregator.GetEvent<LoadingEvent>().Publish(false);
                var content = call.Response != null ? await call.Response.GetStringAsync() : "No Content";
                StratLogger.Information($"[HTTP Response] {call.Request.Verb} {call.Request.Url} - {call.Response?.StatusCode}");
                
                if (call.Succeeded)
                {
                    string? accessToken = null;
                    call.Response?.Headers.TryGetFirst("accesstoken", out accessToken);
                    if (accessToken != null)
                    {
                        TokenManager.SaveToken(accessToken);
                    }
                }
            });
            
            _flurlClient.BeforeCall((call) =>
            {
                _eventAggregator.GetEvent<LoadingEvent>().Publish(true);
                StratLogger.Information($"[HTTP Request] {call.Request.Verb} {call.Request.Url}");
                
                var token = TokenManager.GetToken();
                if (!string.IsNullOrEmpty(token))
                {
                    call.Request.WithHeader("Authorization", $"Bearer {token}");
                }

                if (call.RequestBody != null)
                {
                    StratLogger.Information($"[HTTP Body] {call.RequestBody}");
                }
            });
            
            _flurlClient.OnError(async (action) =>
            {
                _eventAggregator.GetEvent<LoadingEvent>().Publish(false);
                StratLogger.Error($"[HTTP Error] {action.Request.Url}: {action.Exception.Message}");

                var message = action.Exception.Message;
                
                // 打印原始响应内容用于调试（特别是反序列化错误时）
                if (action.Response != null)
                {
                    try
                    {
                        var rawContent = await action.Response.GetStringAsync();
                        StratLogger.Error($"[HTTP Raw Response] 状态码: {action.Response.StatusCode}, 内容: {rawContent}");
                    }
                    catch (Exception ex)
                    {
                        StratLogger.Error($"[HTTP] 无法读取响应内容: {ex.Message}");
                    }
                    
                    if (action.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
                    {
                        TokenManager.ClearToken();
                        _eventAggregator.GetEvent<UnauthorizedEvent>().Publish();
                        return;
                    }
                    else if (action.Response.StatusCode == (int)HttpStatusCode.BadRequest)
                    {
                        message = await action.Response.GetStringAsync();
                    }
                }

                // 发布全局错误消息
                _eventAggregator.GetEvent<AppMessageEvent>().Publish(new AppMessagePayload
                {
                    Title = "系统错误",
                    Content = message,
                    Type = MessageType.Error
                });

                // 尝试从 Response 提取 TraceId (如果是 ABP 标准错误响应)
                string? traceId = null;
                action.Response?.Headers.TryGetFirst("X-Trace-Id", out traceId);

                throw new StratApiException(message, action.Response?.StatusCode ?? 500, 500, traceId);
            });
        }

        public async Task GetAsync(string url, object? data = null)
        {
            var result = await _flurlClient.Request(url).SetQueryParams(data).GetJsonAsync<ApiResponse>();
            if (!result.Success)
            {
                throw new StratApiException(result.Message, 200, result.Code, result.TraceId);
            }
        }

        public async Task<T> GetAsync<T>(string url, object? data = null)
        {
            try
            {
                var result = await _flurlClient.Request(url).SetQueryParams(data).GetJsonAsync<ApiResponse<T>>();
                if (result.Success)
                {
                    return result.Data!;
                }
                throw new StratApiException(result.Message, 200, result.Code, result.TraceId);
            }
            catch (Flurl.Http.FlurlHttpException ex)
            {
                // 捕获反序列化错误，打印原始响应内容用于调试
                if (ex.Call?.Response != null)
                {
                    var rawContent = await ex.Call.Response.GetStringAsync();
                    StratLogger.Error($"[HTTP] API 响应无法反序列化。URL: {url}, 状态码: {ex.Call.Response.StatusCode}, 原始响应: {rawContent}");
                }
                throw;
            }
        }

        public async Task<T> PostAsync<T>(string url, object data)
        {
            var result = await _flurlClient.Request(url).PostJsonAsync(data).ReceiveJson<ApiResponse<T>>();
            if (result.Success)
            {
                return result.Data!;
            }
            throw new StratApiException(result.Message, 200, result.Code, result.TraceId);
        }

        public async Task PostAsync(string url, object data)
        {
            var result = await _flurlClient.Request(url).PostJsonAsync(data).ReceiveJson<ApiResponse>();
            if (!result.Success)
            {
                throw new StratApiException(result.Message, 200, result.Code, result.TraceId);
            }
        }

        public async Task<T> PutAsync<T>(string url, object data)
        {
            var result = await _flurlClient.Request(url).PutJsonAsync(data).ReceiveJson<ApiResponse<T>>();
            if (result.Success)
            {
                return result.Data!;
            }
            throw new StratApiException(result.Message, 200, result.Code, result.TraceId);
        }

        public async Task PutAsync(string url, object data)
        {
            var result = await _flurlClient.Request(url).PutJsonAsync(data).ReceiveJson<ApiResponse>();
            if (!result.Success)
            {
                throw new StratApiException(result.Message, 200, result.Code, result.TraceId);
            }
        }

        public async Task<T> DeleteAsync<T>(string url, object data)
        {
            var result = await _flurlClient.Request(url).SetQueryParams(data).DeleteAsync().ReceiveJson<ApiResponse<T>>();
            if (result.Success)
            {
                return result.Data!;
            }
            throw new StratApiException(result.Message, 200, result.Code, result.TraceId);
        }

        public async Task DeleteAsync(string url, object data)
        {
            var result = await _flurlClient.Request(url).SetQueryParams(data).DeleteAsync().ReceiveJson<ApiResponse>();
            if (!result.Success)
            {
                throw new StratApiException(result.Message, 200, result.Code, result.TraceId);
            }
        }
    }
}

