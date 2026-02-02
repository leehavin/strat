using Prism.Ioc;
using Prism.Modularity;
using Strat.Shared;

namespace Strat.Module.System;

public class SystemModule : IModule
{
    public void OnInitialized(IContainerProvider containerProvider)
    {
    }

    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.AutoRegisterViewForNavigation();
    }
}
