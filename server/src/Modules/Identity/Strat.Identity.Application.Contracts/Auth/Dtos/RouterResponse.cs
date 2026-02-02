namespace Strat.Identity.Application.Contracts.Auth.Dtos;

/// <summary>
/// 路由输出
/// </summary>
public class RouterResponse
{
    /// <summary>
    /// 功能ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 父级ID
    /// </summary>
    public long? ParentId { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 编码
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// 路由路径
    /// </summary>
    public string? Path { get; set; }

    /// <summary>
    /// 组件路径
    /// </summary>
    public string? Component { get; set; }

    /// <summary>
    /// 图标
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// 类型
    /// </summary>
    public int Type { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 是否可见
    /// </summary>
    public bool Visible { get; set; }

    /// <summary>
    /// 子路由
    /// </summary>
    public List<RouterResponse>? Children { get; set; }
}

