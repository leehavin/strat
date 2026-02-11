using Refit;
using Strat.Infrastructure.Services.Refit;
using System.Reflection;

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
            var handler = new StratRefitHandler { InnerHandler = new HttpClientHandler() };
            var httpClient = new HttpClient(handler) { BaseAddress = new System.Uri(baseUrl) };
            
            var settings = new RefitSettings
            {
                ContentSerializer = new SystemTextJsonContentSerializer(new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
                })
            };

            containerRegistry.RegisterInstance(RestService.For<IUserApi>(httpClient, settings));
            containerRegistry.RegisterInstance(RestService.For<IRoleApi>(httpClient, settings));
            containerRegistry.RegisterInstance(RestService.For<IAuthApi>(httpClient, settings));
        }
    }
}

