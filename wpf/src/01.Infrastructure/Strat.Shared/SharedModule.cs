using Flurl.Http.Configuration;
using Microsoft.Extensions.Caching.Memory;
using Strat.Shared.Dialogs;
using Strat.Shared.HttpService;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace Strat.Shared
{
    public class SharedModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance<IMemoryCache>(new MemoryCache(new MemoryCacheOptions()));
            
            // 配置支持反射式序列化的 JsonSerializerOptions
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,  // 忽略大小写
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,  // 服务端返回 camelCase
                TypeInfoResolver = new DefaultJsonTypeInfoResolver()
            };
            var jsonSerializer = new DefaultJsonSerializer(jsonOptions);
            
            containerRegistry.RegisterSingleton<IFlurlClientCache>(sp =>
            {
                var cache = new FlurlClientCache()
                    .Add("strat", "http://localhost:5062/api/v1");
                
                // 获取客户端并配置 JSON 序列化器
                var client = cache.Get("strat");
                client.Settings.JsonSerializer = jsonSerializer;
                
                return cache;
            });
            
            containerRegistry.Register<IStratHttpService, StratHttpService>();
            containerRegistry.RegisterSingleton<IStratDialogService, StratDialogService>();
            
            // 企业级功能服务：通知中心、快捷搜索、主题、国际化
            containerRegistry.RegisterSingleton<Services.INotificationService, Services.NotificationService>();
            containerRegistry.RegisterSingleton<Services.IQuickSearchService, Services.QuickSearchService>();
            containerRegistry.RegisterSingleton<Services.IThemeService, Services.ThemeService>();
            containerRegistry.RegisterSingleton<Services.ILocalizationService, Services.LocalizationService>();
        }
    }
}


