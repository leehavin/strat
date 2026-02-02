using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Prism.DryIoc;
using Strat.Infrastructure.Services.Abstractions;
using Strat.Infrastructure.Services.Implements;
using Strat.Module.Dashboard;
using Strat.Module.Identity;
using Strat.Module.System;
using Strat.Shared.Logging;
using Strat.Shared.Exceptions;
using Strat.UI.Base.Views;

namespace Strat.UI.Base
{
    public partial class App : PrismApplication
    {
        public override void Initialize()
        {
            // 1. 初始化日志系统
            StratLogger.Init();
            
            // 2. 初始化全局异常捕获器（第一阶段：无 DialogService）
            GlobalExceptionHandler.Initialize();
            
            // 3. 加载 XAML
            AvaloniaXamlLoader.Load(this);
            base.Initialize();
        }

        protected override AvaloniaObject CreateShell()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime)
            {
                return Container.Resolve<MainWindow>();
            }
            
            return Container.Resolve<MainView>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // 注册核心壳层视图（仅用于 Browser 平台）
            containerRegistry.RegisterForNavigation<MainView>("MainView");
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<Strat.Shared.SharedModule>();
            moduleCatalog.AddModule<Strat.Infrastructure.InfrastructureModule>();
            moduleCatalog.AddModule<PlatformModule>();
            moduleCatalog.AddModule<IdentityModule>();
            moduleCatalog.AddModule<SystemModule>();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            
            var eventAggregator = Container.Resolve<Prism.Events.IEventAggregator>();
            var dialogService = Container.Resolve<Strat.Shared.Dialogs.IStratDialogService>();
            var regionManager = Container.Resolve<Prism.Navigation.Regions.IRegionManager>();
            
            // 更新全局异常处理器的 DialogService（延迟注入）
            GlobalExceptionHandler.SetDialogService(dialogService);
            
            // 订阅未授权事件，自动跳转到登录页
            eventAggregator.GetEvent<Strat.Shared.Events.UnauthorizedEvent>().Subscribe(() =>
            {
                Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                {
                    regionManager.RequestNavigate("mainRegion", "Login");
                });
            });

            // 订阅全局消息事件
            eventAggregator.GetEvent<Strat.Shared.Events.AppMessageEvent>().Subscribe(async payload =>
            {
                await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(async () =>
                {
                    if (payload.Type == Strat.Shared.Events.MessageType.Error)
                    {
                        await dialogService.ShowErrorAsync(payload.Content, payload.Title);
                    }
                    else
                    {
                        await dialogService.ShowMessageAsync(payload.Content, payload.Title);
                    }
                });
            });
            
            // 无论哪个平台，初始化后都导航到登录
            Avalonia.Threading.Dispatcher.UIThread.Post(() =>
            {
                regionManager.RequestNavigate("mainRegion", "Login");
            });
        }
    }
}
