using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Strat.Shared.Models;

namespace Strat.Host.Filters
{
    /// <summary>
    /// 企业级模型验证过滤器
    /// </summary>
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                var message = string.Join(" | ", errors);
                context.Result = new ObjectResult(ApiResponse.Error(message, 400));
                return;
            }

            await next();
        }
    }
}

