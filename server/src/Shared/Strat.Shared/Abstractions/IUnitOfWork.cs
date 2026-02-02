namespace Strat.Shared.Abstractions;

/// <summary>
/// 工作单元接口
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// 开启事务
    /// </summary>
    void BeginTran();

    /// <summary>
    /// 提交事务
    /// </summary>
    void CommitTran();

    /// <summary>
    /// 回滚事务
    /// </summary>
    void RollbackTran();

    /// <summary>
    /// 执行事务（自动提交/回滚）
    /// </summary>
    Task<T> ExecuteAsync<T>(Func<Task<T>> action);

    /// <summary>
    /// 执行事务（自动提交/回滚）
    /// </summary>
    Task ExecuteAsync(Func<Task> action);
}

