using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Strat.Shared.Exceptions;
using Strat.Shared.Models;
using Volo.Abp;
using Volo.Abp.Http;
using BusinessException = Volo.Abp.BusinessException;

namespace Strat.Host.Filters;

/// <summary>
/// 全局异常过滤器
/// </summary>
public class GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger) : IAsyncExceptionFilter
{
    private readonly ILogger<GlobalExceptionFilter> _logger = logger;

    public Task OnExceptionAsync(ExceptionContext context)
    {
        var exception = context.Exception;
        var statusCode = 500;
        var errorCode = statusCode;
        var message = "服务器内部错误";
        var traceId = Activity.Current?.TraceId.ToString();

        // 响应头添加 TraceId
        if (!string.IsNullOrEmpty(traceId))
        {
            context.HttpContext.Response.Headers.Append("X-Trace-Id", traceId);
        }

        // 1. 处理自定义业务异常
        if (exception is StratException stratException)
        {
            statusCode = 400;
            errorCode = stratException.Code;
            message = stratException.Message;
        }
        // 2. 处理 ABP 业务异常
        else if (exception is UserFriendlyException userFriendlyException)
        {
            statusCode = 400;
            errorCode = int.TryParse(userFriendlyException.Code, out var code) ? code : 400;
            message = userFriendlyException.Message;
        }
        else if (exception is BusinessException businessException)
        {
            statusCode = 400;
            errorCode = int.TryParse(businessException.Code, out var code) ? code : 400;
            message = businessException.Message;
        }
        // 3. 处理参数验证异常 (FluentValidation)
        //else if (exception is ValidationException validationException)
        //{
        //    statusCode = 400;
        //    errorCode = 400;
        //    message = validationException.Message;
        //}
        else
        {
            // 记录未处理异常
            _logger.LogError(exception, "[TraceId:{TraceId}] 未处理异常: {Message}", traceId, exception.Message);
        }

        // 响应头添加 TraceId
        if (!string.IsNullOrEmpty(traceId))
        {
            context.HttpContext.Response.Headers.Append("X-Trace-Id", traceId);
        }

        context.HttpContext.Response.Headers.Append(AbpHttpConsts.AbpErrorFormat, "true");
        context.HttpContext.Response.StatusCode = statusCode;
        context.Result = new ObjectResult(ApiResponse.Error(message, errorCode));
        context.ExceptionHandled = true;

        return Task.CompletedTask;
    }
}

