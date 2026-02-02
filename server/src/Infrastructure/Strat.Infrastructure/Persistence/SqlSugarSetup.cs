using Microsoft.AspNetCore.Http;
using Serilog;
using Strat.Shared.Abstractions;
using Strat.Shared.Constants;
using Yitter.IdGenerator;

namespace Strat.Infrastructure.Persistence;

/// <summary>
/// SqlSugar 配置
/// </summary>
public static class SqlSugarSetup
{
    /// <summary>
    /// 添加 SqlSugar 服务
    /// </summary>
    public static IServiceCollection AddSqlSugar(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["ConnectionOptions:ConnectionString"]
            ?? throw new InvalidOperationException("未配置数据库连接字符串");

        var dbTypeStr = configuration["ConnectionOptions:DbType"] ?? "SqlServer";
        var dbType = Enum.Parse<DbType>(dbTypeStr, true);

        // 注册 HttpContextAccessor
        services.AddHttpContextAccessor();

        // 注册为 Scoped，这样每次请求都会创建一个新的 SqlSugarClient 实例
        // 并且可以安全地注入 IHttpContextAccessor
        services.AddScoped<ISqlSugarClient>(sp =>
        {
            var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();

            SqlSugarClient db = new(new ConnectionConfig
            {
                DbType = dbType,
                ConnectionString = connectionString,
                IsAutoCloseConnection = true
            });

            // 查询过滤器：软删除
            db.QueryFilter.AddTableFilter<ISoftDelete>(it => !it.IsDeleted);

            // 插入和更新过滤器
            db.Aop.DataExecuting = (oldValue, entityInfo) =>
            {
                var userIdStr = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimConst.UserId)?.Value;
                var userId = long.TryParse(userIdStr, out var id) ? id : 0L;
                var now = DateTime.Now;

                if (entityInfo.OperationType == DataFilterType.InsertByObject)
                {
                    if (entityInfo.PropertyName == nameof(BaseEntity.CreateBy))
                        entityInfo.SetValue(userId);

                    if (entityInfo.PropertyName == nameof(BaseEntity.CreateTime))
                        entityInfo.SetValue(now);
                }

                if (entityInfo.OperationType == DataFilterType.UpdateByObject)
                {
                    if (entityInfo.PropertyName == nameof(BaseEntity.UpdateBy))
                        entityInfo.SetValue(userId);

                    if (entityInfo.PropertyName == nameof(BaseEntity.UpdateTime))
                        entityInfo.SetValue(now);
                }
            };

            // SQL 执行日志
            db.Aop.OnLogExecuted = (sql, pars) =>
            {
                if (db.Ado.SqlExecutionTime.TotalMilliseconds > 500)
                {
                    Log.Warning("[慢查询] {Time}ms | {Sql}",
                        db.Ado.SqlExecutionTime.TotalMilliseconds, sql);
                }
            };

            // SQL 错误日志
            db.Aop.OnError = exp =>
            {
                Log.Error(exp, "[SQL错误] {Sql}", exp.Sql);
            };

            return db;
        });

        return services;
    }

    /// <summary>
    /// 添加雪花ID生成器
    /// </summary>
    public static IServiceCollection AddSnowflakeId(this IServiceCollection services, IConfiguration configuration)
    {
        var workId = configuration.GetValue<ushort>("SnowflakeIdOptions:WorkId", 1);

        YitIdHelper.SetIdGenerator(new IdGeneratorOptions(workId));
        StaticConfig.CustomSnowFlakeFunc = YitIdHelper.NextId;

        return services;
    }
}

