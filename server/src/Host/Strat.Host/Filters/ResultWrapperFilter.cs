using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Strat.Shared.Abstractions;
using Strat.Shared.Models;

namespace Strat.Host.Filters;

/// <summary>
/// 结果包装过滤器
/// </summary>
public class ResultWrapperFilter : IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        // 检查是否有 NoWrapper 特性
        if (context.ActionDescriptor is ControllerActionDescriptor actionDescriptor)
        {
            var hasNoWrapper = actionDescriptor.MethodInfo.GetCustomAttributes(typeof(NoWrapperAttribute), true).Any() ||
                               actionDescriptor.ControllerTypeInfo.GetCustomAttributes(typeof(NoWrapperAttribute), true).Any();

            if (hasNoWrapper)
            {
                await next();
                return;
            }
        }

        if (context.Result is ObjectResult objectResult)
        {
            var type = objectResult.Value?.GetType();

            // 如果已经是 ApiResponse 类型，则不再包装
            if (type == null ||
                (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ApiResponse<>)) ||
                type == typeof(ApiResponse))
            {
                await next();
                return;
            }

            objectResult.Value = ApiResponse.Success(objectResult.Value);
        }
        else if (context.Result is EmptyResult)
        {
            context.Result = new ObjectResult(ApiResponse.Success());
        }

        await next();
    }
}

