using Strat.System.Domain.Shared;

namespace Strat.System.Domain.Dict;

/// <summary>
/// 字典数据实体
/// </summary>
[SugarTable("DICT_DATA")]
public class DictDataEntity : BaseEntity
{
    /// <summary>
    /// 字典分类ID
    /// </summary>
    [SugarColumn(ColumnName = "CATEGORY_ID")]
    public long CategoryId { get; set; }

    /// <summary>
    /// 字典名称
    /// </summary>
    [SugarColumn(ColumnName = "NAME")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 字典编码
    /// </summary>
    [SugarColumn(ColumnName = "CODE")]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// 排序
    /// </summary>
    [SugarColumn(ColumnName = "SORT")]
    public int Sort { get; set; }

    #region 导航属性

    [SugarColumn(IsIgnore = true)]
    public string? CategoryName { get; set; }

    #endregion
}

