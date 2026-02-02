namespace Strat.Identity.Application.Contracts.Function.Dtos;

/// <summary>
/// 已绑定接口输出
/// </summary>
public class BindedInterfaceResponse
{
    /// <summary>
    /// 关联ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 接口ID
    /// </summary>
    public long InterfaceId { get; set; }

    /// <summary>
    /// 接口名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 接口路径
    /// </summary>
    public string Path { get; set; } = string.Empty;
}

