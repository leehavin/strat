using Prism.Ioc;
using Refit;
using Strat.Infrastructure.Services.Refit;
using System.Reflection;
using System.Text.Json;
using Strat.Infrastructure.Converters;

namespace Strat.Infrastructure
{
    public class InfrastructureModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // 自动注册所有以 Service 结尾的类及其接口
            var assembly = Assembly.GetExecutingAssembly();
            var serviceTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Service"))
                .ToList();

            foreach (var serviceType in serviceTypes)
            {
                var interfaceType = serviceType.GetInterfaces()
                    .FirstOrDefault(i => i.Name == $"I{serviceType.Name}");

                if (interfaceType != null)
                {
                    containerRegistry.Register(interfaceType, serviceType);
                }
            }

            // Refit API 注册 (第三阶段已激活)
            var baseUrl = "http://localhost:5062/api/v1";
            
            // 配置 Custom JSON Converter
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };
            options.Converters.Add(new DateTimeJsonConverter());
            options.Converters.Add(new NullableDateTimeJsonConverter());
            var refitSettings = new RefitSettings(new SystemTextJsonContentSerializer(options));
            
            // 注册 StratRefitHandler 到容器，让容器注入 IEventAggregator
            containerRegistry.Register<StratRefitHandler>();

            // 使用工厂方法从容器获取已注入 IEventAggregator 的 handler
            containerRegistry.RegisterInstance<IUserApi>(RestService.For<IUserApi>(CreateHttpClient(baseUrl), refitSettings));
            containerRegistry.RegisterInstance<IRoleApi>(RestService.For<IRoleApi>(CreateHttpClient(baseUrl), refitSettings));
            containerRegistry.RegisterInstance<IAuthApi>(RestService.For<IAuthApi>(CreateHttpClient(baseUrl), refitSettings));
            containerRegistry.RegisterInstance<IFunctionApi>(RestService.For<IFunctionApi>(CreateHttpClient(baseUrl), refitSettings));
            containerRegistry.RegisterInstance<IOrganizationApi>(RestService.For<IOrganizationApi>(CreateHttpClient(baseUrl), refitSettings));
        }

        private HttpClient CreateHttpClient(string baseUrl)
        {
            var handler = ContainerLocator.Container.Resolve<StratRefitHandler>();
            handler.InnerHandler = new HttpClientHandler();
            return new HttpClient(handler) { BaseAddress = new System.Uri(baseUrl) };
        }
    }
}

