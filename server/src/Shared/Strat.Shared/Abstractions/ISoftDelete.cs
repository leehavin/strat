namespace Strat.Shared.Abstractions;

/// <summary>
/// 软删除接口
/// </summary>
public interface ISoftDelete
{
    /// <summary>
    /// 是否已删除
    /// </summary>
    bool IsDeleted { get; set; }
}

