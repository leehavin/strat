using System.Diagnostics;
using System.Text;
using Serilog;
using Strat.Shared.Constants;

namespace Strat.Host.Middlewares
{
    /// <summary>
    /// 企业级审计日志中间件
    /// </summary>
    public class AuditLogMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var request = context.Request;
            
            // 记录请求基本信息
            var userIdStr = context.User?.FindFirst(ClaimConst.UserId)?.Value;
            var userId = long.TryParse(userIdStr, out var id) ? id : 0L;
            var userName = context.User?.FindFirst(ClaimConst.UserName)?.Value ?? "Anonymous";

            // 读取请求 Body (如果需要)
            request.EnableBuffering();
            var requestBody = string.Empty;
            if (request.ContentLength > 0 && request.ContentLength < 1024 * 10) // 只记录小于 10KB 的 Body
            {
                using var reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true);
                requestBody = await reader.ReadToEndAsync();
                request.Body.Position = 0;
            }

            await _next(context);

            stopwatch.Stop();
            var statusCode = context.Response.StatusCode;
            var elapsed = stopwatch.ElapsedMilliseconds;

            // 构造审计日志
            Log.Information("[Audit] {Method} {Path} | User: {User}({Id}) | Status: {Status} | Time: {Elapsed}ms | Body: {Body}",
                request.Method,
                request.Path,
                userName,
                userId,
                statusCode,
                elapsed,
                requestBody);
        }
    }

    public static class AuditLogMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuditLog(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuditLogMiddleware>();
        }
    }
}

