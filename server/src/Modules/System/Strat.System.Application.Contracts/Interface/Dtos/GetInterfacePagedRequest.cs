using Strat.Shared.Models;

namespace Strat.System.Application.Contracts.Interface.Dtos;

/// <summary>
/// 接口分页查询输入
/// </summary>
public class GetInterfacePagedRequest : PagedRequest
{
    /// <summary>
    /// 分组名称
    /// </summary>
    public string? GroupName { get; set; }

    /// <summary>
    /// 接口地址
    /// </summary>
    public string? Path { get; set; }
}

