using Strat.Shared.Abstractions;

namespace Strat.Identity.Domain.Shared;

/// <summary>
/// 实体基类
/// </summary>
public abstract class BaseEntity : ISoftDelete
{
    /// <summary>
    /// 主键ID
    /// </summary>
    [SugarColumn(ColumnName = "ID", IsPrimaryKey = true)]
    public long Id { get; set; }

    /// <summary>
    /// 是否删除
    /// </summary>
    [SugarColumn(ColumnName = "IS_DELETED")]
    public bool IsDeleted { get; set; }

    /// <summary>
    /// 创建人
    /// </summary>
    [SugarColumn(ColumnName = "CREATE_BY")]
    public long CreateBy { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [SugarColumn(ColumnName = "CREATE_TIME")]
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 修改人
    /// </summary>
    [SugarColumn(ColumnName = "UPDATE_BY")]
    public long? UpdateBy { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    [SugarColumn(ColumnName = "UPDATE_TIME")]
    public DateTime? UpdateTime { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [SugarColumn(ColumnName = "REMARK")]
    public string? Remark { get; set; }
}

