using Prism.Ioc;
using Prism.Modularity;
using Strat.Shared;
using Strat.Module.Identity.Views;

namespace Strat.Module.Identity;

public class IdentityModule : IModule
{
    public void OnInitialized(IContainerProvider containerProvider)
    {
    }

    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
        // 显式注册视图，确保导航名称匹配
        containerRegistry.RegisterForNavigation<User>("User");
        containerRegistry.AutoRegisterViewForNavigation();
    }
}
