using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation.Regions;
using Strat.Infrastructure.Services.Abstractions;
using Strat.Infrastructure.Models.Auth;
using Strat.Shared.CommonViewModels;
using Strat.Shared.Logging;
using Strat.Shared.Assets;
using Strat.Shared.Services;
using System.Collections.Generic;

namespace Strat.Module.Dashboard.ViewModels
{
    /// <summary>
    /// 主界面 ViewModel（企业级重构版：CQRS + 关注点分离）
    /// </summary>
    public class MainViewModel : StratViewModelBase
    {
        private readonly IRegionManager _regionManager;
        private readonly IAuthService _authService;
        private readonly INotificationService _notificationService;
        private readonly IQuickSearchService _quickSearchService;
        
        // 子 ViewModel（关注点分离）
        public NotificationPanelViewModel NotificationPanel { get; }
        public QuickSearchPanelViewModel QuickSearchPanel { get; }
        
        public MainViewModel(
            IEventAggregator eventAggregator,
            IRegionManager regionManager,
            IAuthService authService,
            INotificationService notificationService,
            IQuickSearchService quickSearchService,
            NotificationPanelViewModel notificationPanel,
            QuickSearchPanelViewModel quickSearchPanel) 
            : base(eventAggregator)
        {
            StratLogger.Information("[MainViewModel] 构造函数开始");
            
            _regionManager = regionManager;
            _authService = authService;
            _notificationService = notificationService;
            _quickSearchService = quickSearchService;
            
            // 注入子 ViewModel
            NotificationPanel = notificationPanel;
            QuickSearchPanel = quickSearchPanel;
            
            MenuItems = new ObservableCollection<MenuItemViewModel>();
            Breadcrumbs = new ObservableCollection<BreadcrumbItem>();
            
            StratLogger.Information("[MainViewModel] 构造函数完成");
            
            // 延迟初始化
            _ = InitializeAsync();
        }
        
        #region Properties
        
        private string _userName = "Administrator";
        /// <summary>
        /// 当前用户名
        /// </summary>
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }
        
        private string _currentPageTitle = "控制台";
        /// <summary>
        /// 当前页面标题
        /// </summary>
        public string CurrentPageTitle
        {
            get => _currentPageTitle;
            set => SetProperty(ref _currentPageTitle, value);
        }
        
        /// <summary>
        /// 菜单项列表
        /// </summary>
        public ObservableCollection<MenuItemViewModel> MenuItems { get; }
        
        /// <summary>
        /// 面包屑导航
        /// </summary>
        public ObservableCollection<BreadcrumbItem> Breadcrumbs { get; }
        
        private object? _selectedMenuItem;
        /// <summary>
        /// 选中的菜单项
        /// </summary>
        public object? SelectedMenuItem
        {
            get => _selectedMenuItem;
            set
            {
                if (SetProperty(ref _selectedMenuItem, value) && value is MenuItemViewModel menuItem)
                {
                    // 只有叶子节点（无子菜单）才导航
                    if (!menuItem.HasChildren)
                    {
                        CurrentPageTitle = menuItem.Name;
                        UpdateBreadcrumbs(menuItem);
                        NavigateToPage(menuItem.Code);
                    }
                }
            }
        }
        
        #endregion
        
        #region Commands
        
        private DelegateCommand? _logoutCommand;
        /// <summary>
        /// 退出登录命令
        /// </summary>
        public DelegateCommand LogoutCommand => 
            _logoutCommand ??= new DelegateCommand(ExecuteLogoutCommand);
        
        private DelegateCommand? _openProfileCommand;
        /// <summary>
        /// 打开个人中心命令
        /// </summary>
        public DelegateCommand OpenProfileCommand => 
            _openProfileCommand ??= new DelegateCommand(ExecuteOpenProfileCommand);
        
        private DelegateCommand? _openSettingsCommand;
        /// <summary>
        /// 打开系统设置命令
        /// </summary>
        public DelegateCommand OpenSettingsCommand => 
            _openSettingsCommand ??= new DelegateCommand(ExecuteOpenSettingsCommand);
        
        #endregion
        
        #region Initialization（查询）
        
        /// <summary>
        /// 异步初始化（CQRS：查询）
        /// </summary>
        private async Task InitializeAsync()
        {
            try
            {
                StratLogger.Information("[Main] 开始异步初始化...");
                
                // 等待视图加载完成
                await Task.Delay(200);
                
                // 并行加载用户信息和路由
                await Task.WhenAll(
                    LoadUserInfoAsync(),
                    LoadRoutersAsync()
                );
            }
            catch (Exception ex)
            {
                StratLogger.Error($"[Main] 初始化异常: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 加载用户信息（查询）
        /// </summary>
        private async Task LoadUserInfoAsync()
        {
            try
            {
                var userInfo = await _authService.GetUserInfoAsync();
                if (userInfo != null && !string.IsNullOrEmpty(userInfo.Name))
                {
                    UserName = userInfo.Name;
                    StratLogger.Information($"[Main] 用户名: {UserName}");
                }
            }
            catch (Exception ex)
            {
                StratLogger.Warning($"[Main] 获取用户信息失败: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 加载动态路由（查询）
        /// </summary>
        private async Task LoadRoutersAsync()
        {
            try
            {
                var routers = await _authService.GetRoutersAsync();
                if (routers != null && routers.Any())
                {
                    StratLogger.Information($"[Main] 获取到 {routers.Count} 个路由节点");
                    
                    global::Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                    {
                        MenuItems.Clear();
                        foreach (var router in routers)
                        {
                            MenuItems.Add(ConvertToMenuItem(router));
                        }
                        
                        // 默认选择第一个子菜单并导航
                        SelectFirstMenuItem();
                    });
                }
                else
                {
                    StratLogger.Warning("[Main] 未获取到路由，使用保底菜单");
                    AddDefaultMenuItems();
                }
            }
            catch (Exception ex)
            {
                StratLogger.Warning($"[Main] 获取路由失败: {ex.Message}");
                AddDefaultMenuItems();
            }
        }
        
        #endregion
        
        #region Menu Management（业务逻辑）
        
        /// <summary>
        /// 菜单代码到 Semi Icons 资源键的映射
        /// </summary>
        private static readonly Dictionary<string, string> IconMap = new()
        {
            { "dashboard", SemiIcons.Dashboard },
            { "welcome", SemiIcons.Home },
            { "system", SemiIcons.Settings },
            { "system.user", SemiIcons.People },
            { "system.role", SemiIcons.Badge },
            { "system.organization", SemiIcons.Business },
            { "system.function", SemiIcons.Work },
            { "system.dictionary", SemiIcons.Book },
            { "system.systemconfig", SemiIcons.Bolt },
            { "system.onlineuser", SemiIcons.Circle },
            { "system.requestlog", SemiIcons.Description },
            { "system.notice", SemiIcons.Campaign },
            { "system.profilesystem", SemiIcons.Assignment },
            { "workflow", SemiIcons.AccountTree },
            { "workflow.definition", SemiIcons.Description },
            { "workflow.my", SemiIcons.Assignment },
            { "workflow.auditing", SemiIcons.CheckCircle },
        };
        
        /// <summary>
        /// 转换路由响应为菜单项
        /// </summary>
        private MenuItemViewModel ConvertToMenuItem(GetRoutersResponse router)
        {
            var iconPath = IconMap.GetValueOrDefault(router.Code, SemiIcons.Description);
            
            var menuItem = new MenuItemViewModel
            {
                Id = router.Id,
                Name = router.Name,
                Code = router.Code,
                ParentId = router.ParentId ?? 0,
                IconPath = iconPath
            };
            
            if (router.Children != null && router.Children.Any())
            {
                foreach (var child in router.Children)
                {
                    menuItem.Children.Add(ConvertToMenuItem(child));
                }
            }
            
            return menuItem;
        }
        
        /// <summary>
        /// 选择第一个可导航的菜单项
        /// </summary>
        private void SelectFirstMenuItem()
        {
            if (MenuItems.Any())
            {
                var firstParent = MenuItems.First();
                if (firstParent.Children.Any())
                {
                    var firstChild = firstParent.Children.First();
                    SelectedMenuItem = firstChild;
                }
                else
                {
                    SelectedMenuItem = firstParent;
                }
            }
        }
        
        /// <summary>
        /// 添加默认菜单项（保底方案）
        /// </summary>
        private void AddDefaultMenuItems()
        {
            global::Avalonia.Threading.Dispatcher.UIThread.Post(() =>
            {
                MenuItems.Clear();
                var defaultItem = new MenuItemViewModel
                {
                    Id = 1,
                    Name = "控制台",
                    Code = "Home",
                    ParentId = 0,
                    IconPath = SemiIcons.Home
                };
                MenuItems.Add(defaultItem);
                SelectedMenuItem = defaultItem;
            });
        }
        
        #endregion
        
        #region Navigation（业务逻辑）
        
        /// <summary>
        /// 更新面包屑导航
        /// </summary>
        private void UpdateBreadcrumbs(MenuItemViewModel currentItem)
        {
            Breadcrumbs.Clear();
            
            // 始终添加首页
            Breadcrumbs.Add(new BreadcrumbItem
            {
                Title = "首页",
                Route = "Home",
                IsLast = false
            });
            
            // 查找父级路径
            var path = new List<MenuItemViewModel>();
            FindMenuPath(MenuItems, currentItem, path);
            
            // 添加路径到面包屑
            for (int i = 0; i < path.Count; i++)
            {
                Breadcrumbs.Add(new BreadcrumbItem
                {
                    Title = path[i].Name,
                    Route = path[i].Code,
                    IsLast = i == path.Count - 1
                });
            }
        }
        
        /// <summary>
        /// 递归查找菜单路径
        /// </summary>
        private bool FindMenuPath(
            ObservableCollection<MenuItemViewModel> items,
            MenuItemViewModel target,
            List<MenuItemViewModel> path)
        {
            foreach (var item in items)
            {
                if (item.Id == target.Id)
                {
                    path.Add(item);
                    return true;
                }
                
                if (item.Children.Any())
                {
                    path.Add(item);
                    if (FindMenuPath(item.Children, target, path))
                    {
                        return true;
                    }
                    path.Remove(item);
                }
            }
            return false;
        }
        
        /// <summary>
        /// 导航到指定页面
        /// </summary>
        private void NavigateToPage(string code)
        {
            StratLogger.Information($"[Main] 尝试导航到: {code}");
            
            var viewName = MapCodeToView(code);
            
            _regionManager.RequestNavigate("MainContentRegion", viewName, result =>
            {
                if (result.Success)
                {
                    StratLogger.Information($"[Main] 导航到 {viewName} 成功");
                }
                else
                {
                    StratLogger.Warning($"[Main] 导航到 {viewName} 失败: {result.Exception?.Message}");
                    _regionManager.RequestNavigate("MainContentRegion", "Home");
                }
            });
        }
        
        /// <summary>
        /// 路由代码映射到视图名称
        /// </summary>
        private string MapCodeToView(string code)
        {
            return code switch
            {
                "welcome" => "Home",
                "system.user" => "User",
                "system.role" => "Role",
                _ => "Home"
            };
        }
        
        #endregion
        
        #region Commands Implementation（命令）
        
        /// <summary>
        /// 执行退出登录（命令）
        /// </summary>
        private void ExecuteLogoutCommand()
        {
            StratLogger.Information("[Main] 用户退出登录");
            _regionManager.RequestNavigate("mainRegion", "Login");
        }
        
        /// <summary>
        /// 打开个人中心（命令）
        /// </summary>
        private void ExecuteOpenProfileCommand()
        {
            StratLogger.Information("[Main] 打开个人中心");
            // TODO: 导航到个人中心页面
            _regionManager.RequestNavigate("MainContentRegion", "Profile");
        }
        
        /// <summary>
        /// 打开系统设置（命令）
        /// </summary>
        private void ExecuteOpenSettingsCommand()
        {
            StratLogger.Information("[Main] 打开系统设置");
            // TODO: 导航到系统设置页面
            _regionManager.RequestNavigate("MainContentRegion", "Settings");
        }
        
        #endregion
    }
}
