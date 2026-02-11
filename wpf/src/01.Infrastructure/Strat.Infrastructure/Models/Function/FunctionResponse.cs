namespace Strat.Infrastructure.Models.Function;

/// <summary>
/// 功能详情输出
/// </summary>
public class FunctionResponse
{
    /// <summary>
    /// 主键ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 功能名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 功能编码
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// 父级ID
    /// </summary>
    public long? ParentId { get; set; }

    /// <summary>
    /// 类型（0:目录 1:菜单 2:按钮）
    /// </summary>
    public int Type { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 图标
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// 路由路径
    /// </summary>
    public string? Path { get; set; }

    /// <summary>
    /// 组件路径
    /// </summary>
    public string? Component { get; set; }

    /// <summary>
    /// 是否可见
    /// </summary>
    public bool Visible { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 子功能列表
    /// </summary>
    public List<FunctionResponse>? Children { get; set; }
}
