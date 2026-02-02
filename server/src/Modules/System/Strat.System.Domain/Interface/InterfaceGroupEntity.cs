using Strat.System.Domain.Shared;

namespace Strat.System.Domain.Interface;

/// <summary>
/// 接口分组实体
/// </summary>
[SugarTable("INTERFACE_GROUP")]
public class InterfaceGroupEntity : BaseEntity
{
    /// <summary>
    /// 分组名称
    /// </summary>
    [SugarColumn(ColumnName = "NAME")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 分组编码
    /// </summary>
    [SugarColumn(ColumnName = "CODE")]
    public string Code { get; set; } = string.Empty;

    #region 导航属性

    /// <summary>
    /// 接口列表
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(InterfaceEntity.GroupId))]
    public List<InterfaceEntity>? Interfaces { get; set; }

    #endregion
}

