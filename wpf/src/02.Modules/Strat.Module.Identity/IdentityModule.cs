using Strat.Module.Identity.ViewModels;
using Strat.Module.Identity.Views;
using Strat.Shared;

namespace Strat.Module.Identity;

public class IdentityModule : IModule
{
    public void OnInitialized(IContainerProvider containerProvider)
    {
    }

    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
        // 显式注册视图与 ViewModel，确保导航和依赖注入 100% 成功
        containerRegistry.RegisterForNavigation<Login, LoginViewModel>("Login");
        
        containerRegistry.RegisterForNavigation<User>("User");
        containerRegistry.RegisterForNavigation<Role>("Role");
        containerRegistry.RegisterDialog<UserEditDialog>("UserEditDialog");
        containerRegistry.RegisterDialog<RoleEditDialog>("RoleEditDialog");
        containerRegistry.AutoRegisterViewForNavigation();
    }
}
