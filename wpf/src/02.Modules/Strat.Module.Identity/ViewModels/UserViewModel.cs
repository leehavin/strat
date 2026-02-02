using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Prism.Commands;
using Prism.Events;
using Strat.Infrastructure.Services.Abstractions;
using Strat.Infrastructure.Models.User;
using Strat.Shared.Models;
using Strat.Shared.CommonViewModels;
using Strat.Infrastructure.Extensions;
using SystemTimer = System.Timers.Timer;

namespace Strat.Module.Identity.ViewModels
{
    /// <summary>
    /// 用户管理 ViewModel（企业级重构版）
    /// 
    /// 架构模式：CQRS（命令查询职责分离）
    /// 关键优化：
    /// 1. 分离查询/命令操作
    /// 2. 防抖搜索机制（500ms）
    /// 3. 完善的异常处理
    /// 4. 取消令牌支持
    /// 5. CanExecute 状态管理
    /// </summary>
    public class UserViewModel : StratViewModelBase
    {
        #region Fields

        private readonly IUserService _userService;
        private readonly SystemTimer _searchDebounceTimer;
        private CancellationTokenSource? _searchCancellationTokenSource;

        #endregion

        #region Properties

        private UserSearchModel _searchModel;
        /// <summary>
        /// 搜索条件模型
        /// </summary>
        public UserSearchModel SearchModel
        {
            get => _searchModel;
            set => SetProperty(ref _searchModel, value);
        }

        private PagedViewModel<UserResponse>? _userPaginationModel;
        /// <summary>
        /// 分页数据模型
        /// </summary>
        public PagedViewModel<UserResponse>? UserPaginationModel
        {
            get => _userPaginationModel;
            set => SetProperty(ref _userPaginationModel, value);
        }

        private bool _isAdvancedSearchOpen;
        /// <summary>
        /// 高级搜索面板是否展开
        /// </summary>
        public bool IsAdvancedSearchOpen
        {
            get => _isAdvancedSearchOpen;
            set => SetProperty(ref _isAdvancedSearchOpen, value);
        }

        private bool _isSearching;
        /// <summary>
        /// 是否正在搜索（用于 CanExecute）
        /// </summary>
        public bool IsSearching
        {
            get => _isSearching;
            set
            {
                if (SetProperty(ref _isSearching, value))
                {
                    SearchCommand.RaiseCanExecuteChanged();
                    ResetCommand.RaiseCanExecuteChanged();
                }
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// 搜索命令
        /// </summary>
        public DelegateCommand SearchCommand { get; }

        /// <summary>
        /// 重置命令
        /// </summary>
        public DelegateCommand ResetCommand { get; }

        /// <summary>
        /// 高级搜索切换命令
        /// </summary>
        public DelegateCommand AdvancedSearchCommand { get; }

        #endregion

        #region Constructor

        public UserViewModel(IEventAggregator eventAggregator, IUserService userService) 
            : base(eventAggregator)
        {
            _userService = userService;
            _searchModel = new UserSearchModel();
            
            // 初始化空的分页模型，防止 XAML 绑定报错
            _userPaginationModel = new PagedViewModel<UserResponse>
            {
                Items = new System.Collections.ObjectModel.ObservableCollection<UserResponse>(),
                Total = 0
            };

            // 初始化防抖定时器（500ms）
            _searchDebounceTimer = new SystemTimer(500);
            _searchDebounceTimer.Elapsed += OnSearchDebounceTimerElapsed;
            _searchDebounceTimer.AutoReset = false;

            // 初始化命令
            SearchCommand = new DelegateCommand(
                executeMethod: ExecuteSearchCommand, 
                canExecuteMethod: () => !IsSearching);
            ResetCommand = new DelegateCommand(
                executeMethod: ExecuteResetCommand, 
                canExecuteMethod: () => !IsSearching);
            AdvancedSearchCommand = new DelegateCommand(() => IsAdvancedSearchOpen = !IsAdvancedSearchOpen);

            // 订阅搜索条件变化
            _searchModel.PropertyChanged += OnSearchModelPropertyChanged;

            // 初始加载
            _ = LoadUsersAsync();
        }

        #endregion

        #region Query Operations (查询操作)

        /// <summary>
        /// 加载用户列表（带取消令牌支持）
        /// </summary>
        private async Task LoadUsersAsync()
        {
            // 取消之前的搜索请求
            _searchCancellationTokenSource?.Cancel();
            _searchCancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = _searchCancellationTokenSource.Token;

            IsSearching = true;

            try
            {
                // 手动映射，避免 Mapster 在 INotifyPropertyChanged 类型上的兼容性问题
                var request = new GetPagedListRequest
                {
                    Account = SearchModel.Account,
                    Name = SearchModel.Name,
                    Telephone = SearchModel.Telephone,
                    Email = SearchModel.Email,
                    Status = SearchModel.Status,
                    PageIndex = SearchModel.PageIndex,
                    PageSize = SearchModel.PageSize
                };
                
                // 调用服务层
                var result = await _userService.GetPagedListAsync(request);
                
                // 检查是否已取消
                if (cancellationToken.IsCancellationRequested)
                    return;

                // 更新 UI 数据
                UserPaginationModel = result.ToUIResult();
            }
            catch (OperationCanceledException)
            {
                // 请求被取消，不做处理（静默忽略）
            }
            catch (Exception ex)
            {
                Strat.Shared.Logging.StratLogger.Error($"[User] 查询用户列表失败: {ex.Message}");
                
                // 初始化空结果防止 UI 报错
                UserPaginationModel = new PagedViewModel<UserResponse>
                {
                    Items = new System.Collections.ObjectModel.ObservableCollection<UserResponse>(),
                    Total = 0
                };
            }
            finally
            {
                IsSearching = false;
            }
        }

        #endregion

        #region Command Implementations (命令实现)

        /// <summary>
        /// 执行搜索命令
        /// </summary>
        private void ExecuteSearchCommand()
        {
            // 重置到第一页
            SearchModel.PageIndex = 1;
            
            // 触发防抖搜索
            TriggerDebounceSearch();
        }

        /// <summary>
        /// 执行重置命令
        /// </summary>
        private void ExecuteResetCommand()
        {
            // 停止防抖定时器
            _searchDebounceTimer.Stop();
            
            // 取消之前的搜索
            _searchCancellationTokenSource?.Cancel();
            
            // 重置搜索条件
            SearchModel = new UserSearchModel();
            
            // 重新订阅事件
            _searchModel.PropertyChanged += OnSearchModelPropertyChanged;
            
            // 立即执行搜索
            _ = LoadUsersAsync();
        }

        #endregion

        #region Event Handlers (事件处理)

        /// <summary>
        /// 搜索条件属性变化事件
        /// </summary>
        private void OnSearchModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // 分页索引改变时立即搜索
            if (e.PropertyName == nameof(PagedParams.PageIndex))
            {
                _ = LoadUsersAsync();
            }
            // 其他条件改变时触发防抖搜索
            else if (e.PropertyName != nameof(PagedParams.PageSize))
            {
                TriggerDebounceSearch();
            }
        }

        /// <summary>
        /// 防抖定时器触发事件
        /// </summary>
        private async void OnSearchDebounceTimerElapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            // 在 UI 线程上执行
            await global::Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(async () =>
            {
                await LoadUsersAsync();
            });
        }

        #endregion

        #region Helper Methods (辅助方法)

        /// <summary>
        /// 触发防抖搜索
        /// </summary>
        private void TriggerDebounceSearch()
        {
            _searchDebounceTimer.Stop();
            _searchDebounceTimer.Start();
        }

        #endregion

        #region Dispose Pattern (资源清理)

        /// <summary>
        /// 析构函数：清理资源
        /// </summary>
        ~UserViewModel()
        {
            _searchDebounceTimer.Elapsed -= OnSearchDebounceTimerElapsed;
            _searchDebounceTimer.Dispose();
            _searchCancellationTokenSource?.Cancel();
            _searchCancellationTokenSource?.Dispose();
            
            if (_searchModel != null)
            {
                _searchModel.PropertyChanged -= OnSearchModelPropertyChanged;
            }
        }

        #endregion
    }

    /// <summary>
    /// 用户搜索条件模型
    /// 
    /// 继承自 PagedParams，包含分页参数和搜索条件
    /// </summary>
    public class UserSearchModel : PagedParams
    {
        private string? _account;
        /// <summary>
        /// 登录账号（模糊搜索）
        /// </summary>
        public string? Account
        {
            get => _account;
            set => SetProperty(ref _account, value);
        }

        private string? _name;
        /// <summary>
        /// 用户名（模糊搜索）
        /// </summary>
        public string? Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string? _telephone;
        /// <summary>
        /// 手机号（模糊搜索）
        /// </summary>
        public string? Telephone
        {
            get => _telephone;
            set => SetProperty(ref _telephone, value);
        }

        private int? _status;
        /// <summary>
        /// 状态（精确匹配）
        /// </summary>
        public int? Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        private DateTimeOffset? _startTime;
        /// <summary>
        /// 创建时间范围 - 开始时间
        /// </summary>
        public DateTimeOffset? StartTime
        {
            get => _startTime;
            set => SetProperty(ref _startTime, value);
        }

        private DateTimeOffset? _endTime;
        /// <summary>
        /// 创建时间范围 - 结束时间
        /// </summary>
        public DateTimeOffset? EndTime
        {
            get => _endTime;
            set => SetProperty(ref _endTime, value);
        }

        private string? _email;
        /// <summary>
        /// 邮箱（模糊搜索）
        /// </summary>
        public string? Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }
    }
}
