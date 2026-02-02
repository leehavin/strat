using Strat.Shared.Abstractions;

namespace Strat.System.Domain.Shared;

/// <summary>
/// 实体基类
/// </summary>
public abstract class BaseEntity : ISoftDelete
{
    [SugarColumn(ColumnName = "ID", IsPrimaryKey = true)]
    public long Id { get; set; }

    [SugarColumn(ColumnName = "IS_DELETED")]
    public bool IsDeleted { get; set; }

    [SugarColumn(ColumnName = "CREATE_BY")]
    public long CreateBy { get; set; }

    [SugarColumn(ColumnName = "CREATE_TIME")]
    public DateTime CreateTime { get; set; }

    [SugarColumn(ColumnName = "UPDATE_BY")]
    public long? UpdateBy { get; set; }

    [SugarColumn(ColumnName = "UPDATE_TIME")]
    public DateTime? UpdateTime { get; set; }

    [SugarColumn(ColumnName = "REMARK")]
    public string? Remark { get; set; }
}

