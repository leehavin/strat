using Strat.Shared.Abstractions;

namespace Strat.Infrastructure.Persistence;

/// <summary>
/// 泛型仓储
/// </summary>
public class Repository<T>(ISqlSugarClient db) : SimpleClient<T>(db), IRepository<T>, ITransientDependency where T : class, new()
{
    public ISugarQueryable<T> Queryable() => Context.Queryable<T>();

    public Task<long> InsertReturnSnowflakeIdAsync(T entity) => Context.Insertable(entity).ExecuteReturnSnowflakeIdAsync();
    /// <summary>
    /// 软删除
    /// </summary>
    public override async Task<bool> DeleteAsync(T entity)
    {
        if (entity is ISoftDelete softDelete)
        {
            softDelete.IsDeleted = true;
            return await Context.Updateable(entity)
                .UpdateColumns(nameof(ISoftDelete.IsDeleted))
                .ExecuteCommandAsync() > 0;
        }

        return await base.DeleteAsync(entity);
    }

    /// <summary>
    /// 根据ID软删除
    /// </summary>
    public override async Task<bool> DeleteByIdAsync(dynamic id)
    {
        if (typeof(ISoftDelete).IsAssignableFrom(typeof(T)))
        {
            return await Context.Updateable<T>()
                .SetColumns(nameof(ISoftDelete.IsDeleted), true)
                .Where($"Id = @id", new { id })
                .ExecuteCommandAsync() > 0;
        }

        return await Context.Deleteable<T>().In(id).ExecuteCommandAsync() > 0;
    }
}

