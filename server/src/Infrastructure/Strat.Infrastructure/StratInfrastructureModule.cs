using Microsoft.Extensions.DependencyInjection;
using Strat.Shared.Abstractions;
using Strat.Infrastructure.Persistence;
using Strat.Infrastructure.Services;
using Strat.Shared;
using Volo.Abp.Modularity;
using System.Reflection;
using System.Linq;

namespace Strat.Infrastructure;

[DependsOn(typeof(StratSharedModule))]
public class StratInfrastructureModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        // 注册通用仓储
        context.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        // 注册事件发布器
        context.Services.AddScoped<IEventPublisher, EventPublisher>();

        // 自动注册所有的事件处理程序 (跨模块扫描)
        RegisterEventHandlers(context.Services);

        // 配置 SqlSugar
        context.Services.AddSqlSugar(configuration);

        // 配置雪花ID
        context.Services.AddSnowflakeId(configuration);
    }

    private void RegisterEventHandlers(IServiceCollection services)
    {
        // 扫描所有已加载的程序集中的 IEventHandler<> 实现
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in assemblies)
        {
            var handlerTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>)));

            foreach (var handlerType in handlerTypes)
            {
                var interfaceTypes = handlerType.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>));

                foreach (var interfaceType in interfaceTypes)
                {
                    services.AddTransient(interfaceType, handlerType);
                }
            }
        }
    }
}

