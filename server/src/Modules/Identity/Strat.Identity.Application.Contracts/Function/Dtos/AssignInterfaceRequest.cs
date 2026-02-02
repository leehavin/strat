namespace Strat.Identity.Application.Contracts.Function.Dtos;

/// <summary>
/// 分配接口输入
/// </summary>
public class AssignInterfaceRequest
{
    /// <summary>
    /// 功能ID
    /// </summary>
    public long FunctionId { get; set; }

    /// <summary>
    /// 接口ID
    /// </summary>
    public long InterfaceId { get; set; }
}

