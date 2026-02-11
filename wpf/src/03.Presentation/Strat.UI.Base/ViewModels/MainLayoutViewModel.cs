using Strat.Shared.CommonViewModels;
using Strat.Shared.Services;
using Strat.Shared.Dialogs;
using Strat.Infrastructure.Services.Abstractions;
using System.Collections.ObjectModel;
using System.Linq;

namespace Strat.UI.Base.ViewModels
{
    public class MainLayoutViewModel : StratViewModelBase
    {
        private readonly Prism.Navigation.Regions.IRegionManager _regionManager;
        private readonly IThemeService _themeService;
        private readonly IAuthService _authService;
        private readonly IStratDialogService _dialogService;

        private string _userName = "Administrator";
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        private DelegateCommand? _logoutCommand;
        public DelegateCommand LogoutCommand => _logoutCommand ??= new DelegateCommand(ExecuteLogout);

        private async void ExecuteLogout()
        {
            var confirm = await _dialogService.ShowConfirmAsync("确定要退出登录吗？", "退出确认");
            if (confirm)
            {
                await _authService.LogoutAsync();
                _regionManager.RequestNavigate("mainRegion", "Login");
            }
        }

        public MainLayoutViewModel(
            IEventAggregator eventAggregator, 
            Prism.Navigation.Regions.IRegionManager regionManager,
            IThemeService themeService,
            IAuthService authService,
            IStratDialogService dialogService) : base(eventAggregator)
        {
            _regionManager = regionManager;
            _themeService = themeService;
            _authService = authService;
            _dialogService = dialogService;
            Title = "Strat Admin";
            InitMenus();
        }

        public override async void OnNavigatedTo(Prism.Navigation.Regions.NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            await LoadDynamicMenus();
            await LoadUserInfo();
        }

        private async Task LoadUserInfo()
        {
            try
            {
                var info = await _authService.GetUserInfoAsync();
                if (info != null)
                {
                    UserName = info.Name;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load user info: {ex.Message}");
            }
        }

        private MenuItem? FindFirstLeafMenu(IEnumerable<MenuItem> menus)
        {
            foreach (var menu in menus)
            {
                if (!string.IsNullOrEmpty(menu.ViewName))
                {
                    return menu;
                }
                if (menu.Items.Count > 0)
                {
                    var leaf = FindFirstLeafMenu(menu.Items);
                    if (leaf != null) return leaf;
                }
            }
            return null;
        }

        private async Task LoadDynamicMenus()
        {
            try
            {
                var routers = await _authService.GetRoutersAsync();
                
                // 强制在 UI 线程执行更新
                await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
                {
                    MenuItems.Clear();
                    
                    // 1. 如果有远程菜单，则加载远程菜单
                    if (routers != null && routers.Count > 0)
                    {
                        var sortedRouters = routers.OrderBy(r => r.Sort);
                        foreach (var router in sortedRouters)
                        {
                            var menu = MapRouterToMenu(router);
                            if (menu != null)
                            {
                                MenuItems.Add(menu);
                            }
                        }
                    }
                    
                    // 2. 如果菜单仍然为空（或者为了确保基础菜单存在），添加保底菜单
                    if (MenuItems.Count == 0)
                    {
                        InitMenus();
                    }

                    // 3. 默认导航到第一个有效的叶子菜单
                    var firstLeaf = FindFirstLeafMenu(MenuItems);
                    if (firstLeaf != null)
                    {
                        SelectedMenu = firstLeaf; // 通过设置 SelectedMenu 触发导航
                    }
                });
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load dynamic menus: {ex.Message}");
                // 出错时加载静态菜单作为回退
                await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() => {
                    MenuItems.Clear();
                    InitMenus();
                });
            }
        }

        private MenuItem? MapRouterToMenu(Strat.Infrastructure.Models.Auth.GetRoutersResponse router)
        {
            // 1. 如果菜单不可见，直接过滤
            if (!router.Visible)
            {
                return null;
            }

            // 2. 如果是按钮类型（Type=2），不应该出现在菜单中
            if (router.Type == 2)
            {
                return null;
            }

            // 优先使用后端返回的 Component 作为 ViewName，如果为空则尝试通过 Code 映射
            var viewName = !string.IsNullOrEmpty(router.Component) ? router.Component : MapCodeToView(router.Code);
            
            // 优先使用后端返回的 Icon，如果为空则尝试通过 Code 映射
            var icon = !string.IsNullOrEmpty(router.Icon) ? router.Icon : GetIconByCode(router.Code);

            var menu = new MenuItem 
            { 
                Name = router.Name, 
                ViewName = viewName,
                Icon = icon
            };

            if (router.Children != null && router.Children.Count > 0)
            {
                // 对子菜单进行排序
                var sortedChildren = router.Children.OrderBy(c => c.Sort);
                foreach (var child in sortedChildren)
                {
                    // 递归映射子菜单
                    var childMenu = MapRouterToMenu(child);
                    if (childMenu != null)
                    {
                        menu.Items.Add(childMenu);
                    }
                }
            }
            
            // 策略：
            // 1. 如果有 ViewName，说明是功能节点，返回
            // 2. 如果没有 ViewName 但有子项，说明是目录节点，返回
            // 3. 否则是无效节点，返回 null
            if (string.IsNullOrEmpty(menu.ViewName) && menu.Items.Count == 0)
            {
                return null;
            }

            return menu;
        }

        private string MapCodeToView(string code)
        {
            // 兼容旧的硬编码映射，逐步迁移到数据库配置 Component
            return code switch
            {
                "welcome" => "Home",
                "system.user" => "UserManagementView",
                "system.role" => "RoleManagementView",
                "system.dept" => "OrganizationManagementView", 
                "system.menu" => "InterfaceManagementView", 
                "system.dict" => "DictManagementView",
                "system.config" => "SystemConfigView",
                _ => ""
            };
        }

        private string GetIconByCode(string code)
        {
            // 尽量匹配 Semi Icons 命名
            return code switch
            {
                "dashboard" => "Home",
                "welcome" => "Home",
                "system" => "Setting",
                "system.user" => "User",
                "system.role" => "Lock",
                "system.organization" => "UserGroup",
                "system.function" => "Menu",
                "system.dictionary" => "Book",
                "system.systemconfig" => "Setting",
                "system.requestlog" => "Article",
                "system.notice" => "Bell",
                "system.profilesystem" => "Folder",
                "workflow" => "Branch",
                "workflow.definition" => "Edit",
                "workflow.my" => "User",
                "workflow.auditing" => "Tick",
                _ => "Setting"
            };
        }

        private bool _isPaneOpen = true;
        public bool IsPaneOpen
        {
            get => _isPaneOpen;
            set => SetProperty(ref _isPaneOpen, value);
        }

        private DelegateCommand? _togglePaneCommand;
        public DelegateCommand TogglePaneCommand => _togglePaneCommand ??= new DelegateCommand(() => IsPaneOpen = !IsPaneOpen);

        private MenuItem? _selectedMenu;
        public MenuItem? SelectedMenu
        {
            get => _selectedMenu;
            set
            {
                if (SetProperty(ref _selectedMenu, value) && value != null && !string.IsNullOrEmpty(value.ViewName))
                {
                    NavigateCommand.Execute(value);
                }
            }
        }

        public ObservableCollection<MenuItem> MenuItems { get; } = new();
        public ObservableCollection<TabItemModel> OpenTabs { get; } = new();

        private TabItemModel? _currentTab;
        public TabItemModel? CurrentTab
        {
            get => _currentTab;
            set
            {
                if (SetProperty(ref _currentTab, value) && value != null)
                {
                    _regionManager.RequestNavigate("ContentRegion", value.ViewName);
                    UpdateBreadcrumbs(value);
                    SyncSelectedMenu(value.ViewName);
                }
            }
        }

        private void SyncSelectedMenu(string viewName)
        {
            SelectedMenu = FindMenuByViewName(MenuItems, viewName);
        }

        private MenuItem? FindMenuByViewName(IEnumerable<MenuItem> menus, string viewName)
        {
            foreach (var menu in menus)
            {
                if (menu.ViewName == viewName) return menu;
                var child = FindMenuByViewName(menu.Items, viewName);
                if (child != null) return child;
            }
            return null;
        }

        public ObservableCollection<BreadcrumbItem> Breadcrumbs { get; } = new();

        private DelegateCommand<MenuItem>? _navigateCommand;
        public DelegateCommand<MenuItem> NavigateCommand => _navigateCommand ??= new DelegateCommand<MenuItem>(ExecuteNavigate);

        private DelegateCommand? _toggleThemeCommand;
        public DelegateCommand ToggleThemeCommand => _toggleThemeCommand ??= new DelegateCommand(() => 
        {
            _themeService.ToggleTheme();
            RaisePropertyChanged(nameof(CurrentTheme));
        });

        public ThemeMode CurrentTheme => _themeService.CurrentMode;

        private DelegateCommand<TabItemModel>? _closeTabCommand;
        public DelegateCommand<TabItemModel> CloseTabCommand => _closeTabCommand ??= new DelegateCommand<TabItemModel>(ExecuteCloseTab);

        private DelegateCommand? _closeAllTabsCommand;
        public DelegateCommand CloseAllTabsCommand => _closeAllTabsCommand ??= new DelegateCommand(ExecuteCloseAllTabs);

        private DelegateCommand<TabItemModel>? _closeOtherTabsCommand;
        public DelegateCommand<TabItemModel> CloseOtherTabsCommand => _closeOtherTabsCommand ??= new DelegateCommand<TabItemModel>(ExecuteCloseOtherTabs);

        private void ExecuteCloseTab(TabItemModel tab)
        {
            if (tab.ViewName == "Home") return;
            
            int index = OpenTabs.IndexOf(tab);
            OpenTabs.Remove(tab);
            
            if (CurrentTab == tab)
            {
                var nextTab = OpenTabs.Count > index ? OpenTabs[index] : OpenTabs.LastOrDefault();
                
                // 关键修复：即使 nextTab 是同一个引用，也要强制触发导航
                _currentTab = null; // 先清空，确保 setter 逻辑能被触发
                CurrentTab = nextTab;
                
                if (nextTab != null)
                {
                    _regionManager.RequestNavigate("ContentRegion", nextTab.ViewName);
                }
                else
                {
                    // 如果没有任何 Tab，回退到首页
                    _regionManager.RequestNavigate("ContentRegion", "Home");
                }
            }
        }

        private void ExecuteCloseAllTabs()
        {
            var homeTab = OpenTabs.FirstOrDefault(t => t.ViewName == "Home");
            OpenTabs.Clear();
            if (homeTab != null) OpenTabs.Add(homeTab);
            CurrentTab = homeTab;
            _regionManager.RequestNavigate("ContentRegion", "Home");
        }

        private void ExecuteCloseOtherTabs(TabItemModel tab)
        {
            var homeTab = OpenTabs.FirstOrDefault(t => t.ViewName == "Home");
            OpenTabs.Clear();
            if (homeTab != null) OpenTabs.Add(homeTab);
            if (tab != homeTab) OpenTabs.Add(tab);
            CurrentTab = tab;
            _regionManager.RequestNavigate("ContentRegion", tab.ViewName);
        }

        private void ExecuteNavigate(MenuItem menu)
        {
            if (string.IsNullOrEmpty(menu.ViewName)) return;

            var existingTab = OpenTabs.FirstOrDefault(t => t.ViewName == menu.ViewName);
            if (existingTab == null)
            {
                existingTab = new TabItemModel { Header = menu.Name, ViewName = menu.ViewName, Icon = menu.Icon, CanClose = menu.ViewName != "Home" };
                OpenTabs.Add(existingTab);
            }
            
            // 无论是否是同一个 Tab，都强制触发一次导航确保视图同步
            CurrentTab = existingTab;
            _regionManager.RequestNavigate("ContentRegion", menu.ViewName);
        }

        private void UpdateBreadcrumbs(TabItemModel tab)
        {
            Breadcrumbs.Clear();
            Breadcrumbs.Add(new BreadcrumbItem { Title = "Home", Route = "Home" });
            if (tab.ViewName != "Home")
            {
                Breadcrumbs.Add(new BreadcrumbItem { Title = tab.Header, Route = tab.ViewName, IsLast = true });
            }
            
            // 当 Tab 切换时，也要确保 Region 导航同步
            _regionManager.RequestNavigate("ContentRegion", tab.ViewName);
        }

        private void InitMenus()
        {
            MenuItems.Add(new MenuItem { Name = "Dashboard", Icon = "Home", ViewName = "Home" });
            
            var sysMenu = new MenuItem { Name = "System", Icon = "Setting" };
            sysMenu.Items.Add(new MenuItem { Name = "Users", Icon = "User", ViewName = "UserManagementView" });
            sysMenu.Items.Add(new MenuItem { Name = "Roles", Icon = "Lock", ViewName = "RoleManagementView" });
            
            MenuItems.Add(sysMenu);
        }
    }

    public class TabItemModel : BindableBase
    {
        public string Header { get; set; } = string.Empty;
        public string ViewName { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public bool CanClose { get; set; } = true;
    }

    public class MenuItem : BindableBase
    {
        public string Name { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string? ViewName { get; set; }
        public ObservableCollection<MenuItem> Items { get; } = new();
    }
}
