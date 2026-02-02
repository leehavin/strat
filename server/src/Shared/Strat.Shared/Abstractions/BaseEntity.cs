namespace Strat.Shared.Abstractions;

/// <summary>
/// 实体基类（用于 SqlSugar AOP 字段名检测）
/// </summary>
public abstract class BaseEntity : ISoftDelete
{
    /// <summary>
    /// 主键ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 是否删除
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// 创建人
    /// </summary>
    public long CreateBy { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 修改人
    /// </summary>
    public long? UpdateBy { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    public DateTime? UpdateTime { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }
}

