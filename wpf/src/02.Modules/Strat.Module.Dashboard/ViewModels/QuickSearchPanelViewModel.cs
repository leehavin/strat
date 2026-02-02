using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation.Regions;
using Strat.Shared.CommonViewModels;
using Strat.Shared.Models;
using Strat.Shared.Services;
using Strat.Shared.Logging;
using SystemTimer = System.Timers.Timer;

namespace Strat.Module.Dashboard.ViewModels
{
    /// <summary>
    /// 快捷搜索面板 ViewModel（企业级命令面板实现）
    /// </summary>
    public class QuickSearchPanelViewModel : StratViewModelBase
    {
        private readonly IQuickSearchService _searchService;
        private readonly IRegionManager _regionManager;
        private readonly SystemTimer _debounceTimer;
        
        public QuickSearchPanelViewModel(
            IEventAggregator eventAggregator,
            IQuickSearchService searchService,
            IRegionManager regionManager) 
            : base(eventAggregator)
        {
            _searchService = searchService;
            _regionManager = regionManager;
            
            SearchResults = new ObservableCollection<QuickSearchItem>();
            
            // 初始化防抖定时器（300ms）
            _debounceTimer = new SystemTimer(300);
            _debounceTimer.Elapsed += OnDebounceTimerElapsed;
            _debounceTimer.AutoReset = false;
            
            // 初始加载最近使用
            _ = LoadRecentItemsAsync();
        }
        
        #region Properties
        
        private bool _isOpen;
        /// <summary>
        /// 面板是否打开
        /// </summary>
        public bool IsOpen
        {
            get => _isOpen;
            set
            {
                if (SetProperty(ref _isOpen, value))
                {
                    if (value)
                    {
                        // 打开时重置搜索
                        SearchKeyword = string.Empty;
                        _ = LoadRecentItemsAsync();
                    }
                }
            }
        }
        
        private string _searchKeyword = string.Empty;
        /// <summary>
        /// 搜索关键词
        /// </summary>
        public string SearchKeyword
        {
            get => _searchKeyword;
            set
            {
                if (SetProperty(ref _searchKeyword, value))
                {
                    // 防抖搜索
                    _debounceTimer.Stop();
                    _debounceTimer.Start();
                }
            }
        }
        
        private QuickSearchItem? _selectedItem;
        /// <summary>
        /// 选中的项目
        /// </summary>
        public QuickSearchItem? SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }
        
        private int _selectedIndex;
        /// <summary>
        /// 选中的索引（用于键盘导航）
        /// </summary>
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                if (SetProperty(ref _selectedIndex, value) && value >= 0 && value < SearchResults.Count)
                {
                    SelectedItem = SearchResults[value];
                }
            }
        }
        
        /// <summary>
        /// 搜索结果列表
        /// </summary>
        public ObservableCollection<QuickSearchItem> SearchResults { get; }
        
        #endregion
        
        #region Commands
        
        private DelegateCommand? _togglePanelCommand;
        /// <summary>
        /// 切换面板显示/隐藏（Ctrl+K）
        /// </summary>
        public DelegateCommand TogglePanelCommand => 
            _togglePanelCommand ??= new DelegateCommand(() => IsOpen = !IsOpen);
        
        private DelegateCommand<QuickSearchItem>? _executeItemCommand;
        /// <summary>
        /// 执行选中的项目
        /// </summary>
        public DelegateCommand<QuickSearchItem> ExecuteItemCommand => 
            _executeItemCommand ??= new DelegateCommand<QuickSearchItem>(
                async item => await ExecuteItemAsync(item),
                item => item != null);
        
        private DelegateCommand? _executeSelectedCommand;
        /// <summary>
        /// 执行当前选中项（Enter键）
        /// </summary>
        public DelegateCommand ExecuteSelectedCommand => 
            _executeSelectedCommand ??= new DelegateCommand(
                async () => await ExecuteItemAsync(SelectedItem),
                () => SelectedItem != null)
            .ObservesProperty(() => SelectedItem);
        
        private DelegateCommand? _moveUpCommand;
        /// <summary>
        /// 向上移动选择（Up键）
        /// </summary>
        public DelegateCommand MoveUpCommand => 
            _moveUpCommand ??= new DelegateCommand(() =>
            {
                if (SelectedIndex > 0)
                {
                    SelectedIndex--;
                }
            });
        
        private DelegateCommand? _moveDownCommand;
        /// <summary>
        /// 向下移动选择（Down键）
        /// </summary>
        public DelegateCommand MoveDownCommand => 
            _moveDownCommand ??= new DelegateCommand(() =>
            {
                if (SelectedIndex < SearchResults.Count - 1)
                {
                    SelectedIndex++;
                }
            });
        
        #endregion
        
        #region Methods
        
        /// <summary>
        /// 防抖定时器触发
        /// </summary>
        private async void OnDebounceTimerElapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            await PerformSearchAsync();
        }
        
        /// <summary>
        /// 执行搜索
        /// </summary>
        private async Task PerformSearchAsync()
        {
            try
            {
                var results = string.IsNullOrWhiteSpace(SearchKeyword)
                    ? await _searchService.GetRecentItemsAsync()
                    : await _searchService.SearchAsync(SearchKeyword, maxResults: 10);
                
                // 更新UI
                await global::Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
                {
                    SearchResults.Clear();
                    foreach (var item in results)
                    {
                        SearchResults.Add(item);
                    }
                    
                    // 自动选中第一项
                    SelectedIndex = SearchResults.Any() ? 0 : -1;
                });
                
                StratLogger.Information($"[QuickSearch] 搜索完成，关键词: {SearchKeyword}, 结果数: {results.Count}");
            }
            catch (Exception ex)
            {
                StratLogger.Error($"[QuickSearch] 搜索失败: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 加载最近使用项目
        /// </summary>
        private async Task LoadRecentItemsAsync()
        {
            try
            {
                var recentItems = await _searchService.GetRecentItemsAsync(5);
                
                await global::Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
                {
                    SearchResults.Clear();
                    foreach (var item in recentItems)
                    {
                        SearchResults.Add(item);
                    }
                    
                    SelectedIndex = SearchResults.Any() ? 0 : -1;
                });
            }
            catch (Exception ex)
            {
                StratLogger.Error($"[QuickSearch] 加载最近使用失败: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 执行项目动作
        /// </summary>
        private async Task ExecuteItemAsync(QuickSearchItem? item)
        {
            if (item == null) return;
            
            try
            {
                // 记录使用历史
                await _searchService.RecordUsageAsync(item.Id);
                
                // 根据类型执行不同的动作
                switch (item.Type)
                {
                    case QuickSearchType.Page:
                        // 页面导航
                        if (!string.IsNullOrEmpty(item.TargetRoute))
                        {
                            _regionManager.RequestNavigate("MainContentRegion", item.TargetRoute, result =>
                            {
                                if (result.Success)
                                {
                                    StratLogger.Information($"[QuickSearch] 导航成功: {item.TargetRoute}");
                                }
                                else
                                {
                                    StratLogger.Warning($"[QuickSearch] 导航失败: {result.Exception?.Message}");
                                }
                            });
                        }
                        break;
                    
                    case QuickSearchType.Action:
                        // 执行动作
                        item.Action?.Invoke();
                        StratLogger.Information($"[QuickSearch] 执行动作: {item.Title}");
                        break;
                    
                    case QuickSearchType.Setting:
                        // 打开设置页面
                        StratLogger.Information($"[QuickSearch] 打开设置: {item.Title}");
                        break;
                    
                    default:
                        StratLogger.Warning($"[QuickSearch] 未知类型: {item.Type}");
                        break;
                }
                
                // 关闭面板
                IsOpen = false;
            }
            catch (Exception ex)
            {
                StratLogger.Error($"[QuickSearch] 执行项目失败: {ex.Message}");
            }
        }
        
        #endregion
        
        /// <summary>
        /// 清理资源
        /// </summary>
        ~QuickSearchPanelViewModel()
        {
            _debounceTimer?.Dispose();
        }
    }
}
