using Strat.Identity.Domain.Shared;

namespace Strat.Identity.Domain.Function;

/// <summary>
/// 功能实体
/// </summary>
[SugarTable("FUNCTION")]
public class FunctionEntity : BaseEntity
{
    /// <summary>
    /// 功能名称
    /// </summary>
    [SugarColumn(ColumnName = "NAME")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 功能编码
    /// </summary>
    [SugarColumn(ColumnName = "CODE")]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// 父级ID
    /// </summary>
    [SugarColumn(ColumnName = "PARENT_ID")]
    public long? ParentId { get; set; }

    /// <summary>
    /// 类型（0:目录 1:菜单 2:按钮）
    /// </summary>
    [SugarColumn(ColumnName = "TYPE")]
    public int Type { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [SugarColumn(ColumnName = "SORT")]
    public int Sort { get; set; }

    /// <summary>
    /// 图标
    /// </summary>
    [SugarColumn(ColumnName = "ICON")]
    public string? Icon { get; set; }

    /// <summary>
    /// 路由路径
    /// </summary>
    [SugarColumn(ColumnName = "PATH")]
    public string? Path { get; set; }

    /// <summary>
    /// 组件路径
    /// </summary>
    [SugarColumn(ColumnName = "COMPONENT")]
    public string? Component { get; set; }

    /// <summary>
    /// 是否可见
    /// </summary>
    [SugarColumn(ColumnName = "VISIBLE")]
    public bool Visible { get; set; } = true;

    #region 导航属性

    /// <summary>
    /// 子功能列表
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public List<FunctionEntity>? Children { get; set; }

    #endregion
}

