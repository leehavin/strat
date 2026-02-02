using System.Diagnostics;
using Serilog.Context;

namespace Strat.Host.Middlewares;

/// <summary>
/// TraceId 中间件
/// </summary>
public class TraceIdMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        var traceId = Activity.Current?.TraceId.ToString() ?? Guid.NewGuid().ToString("N");

        // 添加到响应头
        context.Response.OnStarting(() =>
        {
            context.Response.Headers.TryAdd("X-Trace-Id", traceId);
            return Task.CompletedTask;
        });

        // 添加到 Serilog 日志上下文
        using (LogContext.PushProperty("TraceId", traceId))
        {
            await _next(context);
        }
    }
}

/// <summary>
/// 扩展方法
/// </summary>
public static class TraceIdMiddlewareExtensions
{
    public static IApplicationBuilder UseTraceId(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<TraceIdMiddleware>();
    }
}

