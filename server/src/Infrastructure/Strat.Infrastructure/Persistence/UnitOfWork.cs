using Strat.Shared.Abstractions;

namespace Strat.Infrastructure.Persistence;

/// <summary>
/// 工作单元实现
/// </summary>
public class UnitOfWork(ISqlSugarClient db) : IUnitOfWork, ITransientDependency
{
    private readonly ISqlSugarClient _db = db;

    /// <summary>
    /// 开启事务
    /// </summary>
    public void BeginTran()
    {
        _db.Ado.BeginTran();
    }

    /// <summary>
    /// 提交事务
    /// </summary>
    public void CommitTran()
    {
        _db.Ado.CommitTran();
    }

    /// <summary>
    /// 回滚事务
    /// </summary>
    public void RollbackTran()
    {
        _db.Ado.RollbackTran();
    }

    /// <summary>
    /// 执行事务（自动提交/回滚）
    /// </summary>
    public async Task<T> ExecuteAsync<T>(Func<Task<T>> action)
    {
        try
        {
            BeginTran();
            var result = await action();
            CommitTran();
            return result;
        }
        catch
        {
            RollbackTran();
            throw;
        }
    }

    /// <summary>
    /// 执行事务（自动提交/回滚）
    /// </summary>
    public async Task ExecuteAsync(Func<Task> action)
    {
        try
        {
            BeginTran();
            await action();
            CommitTran();
        }
        catch
        {
            RollbackTran();
            throw;
        }
    }
}

