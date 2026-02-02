namespace Strat.System.Application.Contracts.Interface.Dtos;

/// <summary>
/// 接口输出
/// </summary>
public class InterfaceResponse
{
    /// <summary>
    /// ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 接口名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 接口地址
    /// </summary>
    public string Path { get; set; } = string.Empty;

    /// <summary>
    /// 请求方法
    /// </summary>
    public string RequestMethod { get; set; } = string.Empty;

    /// <summary>
    /// 分组ID
    /// </summary>
    public long GroupId { get; set; }
}

