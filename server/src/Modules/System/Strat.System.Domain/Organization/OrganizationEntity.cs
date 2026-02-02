using Strat.System.Domain.Shared;

namespace Strat.System.Domain.Organization;

/// <summary>
/// 组织架构实体
/// </summary>
[SugarTable("ORGANIZATION")]
public class OrganizationEntity : BaseEntity
{
    /// <summary>
    /// 组织名称
    /// </summary>
    [SugarColumn(ColumnName = "NAME")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 父级ID
    /// </summary>
    [SugarColumn(ColumnName = "PARENT_ID")]
    public long? ParentId { get; set; }

    /// <summary>
    /// 联系电话
    /// </summary>
    [SugarColumn(ColumnName = "TELEPHONE")]
    public string? Telephone { get; set; }

    /// <summary>
    /// 负责人
    /// </summary>
    [SugarColumn(ColumnName = "LEADER")]
    public string? Leader { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [SugarColumn(ColumnName = "SORT")]
    public int Sort { get; set; }

    #region 导航属性

    [SugarColumn(IsIgnore = true)]
    public List<OrganizationEntity>? Children { get; set; }

    #endregion
}

