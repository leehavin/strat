using Strat.System.Domain.Shared;

namespace Strat.System.Domain.Dict;

/// <summary>
/// 字典分类实体
/// </summary>
[SugarTable("DICT_CATEGORY")]
public class DictCategoryEntity : BaseEntity
{
    /// <summary>
    /// 分类名称
    /// </summary>
    [SugarColumn(ColumnName = "NAME")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 分类编码
    /// </summary>
    [SugarColumn(ColumnName = "CODE")]
    public string Code { get; set; } = string.Empty;
}

