using Prism.Ioc;
using Prism.Modularity;
using Strat.Module.Dashboard.ViewModels;
using Strat.Module.Dashboard.Views;
using Strat.Shared;

namespace Strat.Module.Dashboard
{
    public class PlatformModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.AutoRegisterViewForNavigation();
            
            // 注册子 ViewModel（非导航视图）
            containerRegistry.Register<NotificationPanelViewModel>();
            containerRegistry.Register<QuickSearchPanelViewModel>();
        }
    }
}
