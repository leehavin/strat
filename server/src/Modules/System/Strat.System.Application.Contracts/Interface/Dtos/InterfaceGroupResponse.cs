namespace Strat.System.Application.Contracts.Interface.Dtos;

/// <summary>
/// 接口分组输出
/// </summary>
public class InterfaceGroupResponse
{
    /// <summary>
    /// ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 分组名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 分组编码
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// 接口列表
    /// </summary>
    public List<InterfaceResponse>? Interfaces { get; set; }
}

