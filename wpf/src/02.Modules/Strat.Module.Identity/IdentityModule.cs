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
        containerRegistry.RegisterForNavigation<Login, LoginViewModel>("Login");
        containerRegistry.AutoRegisterViewForNavigation();
    }
}
