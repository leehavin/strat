using SqlSugar;

namespace Strat.Shared.Abstractions;

/// <summary>
/// 仓储接口
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IRepository<T> where T : class, new()
{
    ISqlSugarClient Context { get; }
    
    ISugarQueryable<T> Queryable();
    
    Task<T> GetByIdAsync(dynamic id);
    
    Task<List<T>> GetListAsync();
    
    Task<bool> InsertAsync(T entity);
    
    Task<long> InsertReturnSnowflakeIdAsync(T entity);
    
    Task<bool> UpdateAsync(T entity);
    
    Task<bool> DeleteAsync(T entity);
    
    Task<bool> DeleteByIdAsync(dynamic id);
}

