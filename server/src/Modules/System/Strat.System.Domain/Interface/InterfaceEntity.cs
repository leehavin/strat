using Strat.System.Domain.Shared;

namespace Strat.System.Domain.Interface;

/// <summary>
/// 接口实体
/// </summary>
[SugarTable("INTERFACE")]
public class InterfaceEntity : BaseEntity
{
    /// <summary>
    /// 接口名称
    /// </summary>
    [SugarColumn(ColumnName = "NAME")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 接口路径
    /// </summary>
    [SugarColumn(ColumnName = "PATH")]
    public string Path { get; set; } = string.Empty;

    /// <summary>
    /// 请求方法
    /// </summary>
    [SugarColumn(ColumnName = "REQUEST_METHOD")]
    public string RequestMethod { get; set; } = string.Empty;

    /// <summary>
    /// 接口分组ID
    /// </summary>
    [SugarColumn(ColumnName = "GROUP_ID")]
    public long GroupId { get; set; }

    #region 导航属性

    [SugarColumn(IsIgnore = true)]
    public string? GroupName { get; set; }

    #endregion
}

