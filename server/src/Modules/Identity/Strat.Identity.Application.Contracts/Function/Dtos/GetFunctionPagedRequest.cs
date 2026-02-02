using Strat.Shared.Models;

namespace Strat.Identity.Application.Contracts.Function.Dtos;

/// <summary>
/// 功能分页查询输入
/// </summary>
public class GetFunctionPagedRequest : PagedRequest
{
    /// <summary>
    /// 功能名称（模糊查询）
    /// </summary>
    public string? Name { get; set; }
}

