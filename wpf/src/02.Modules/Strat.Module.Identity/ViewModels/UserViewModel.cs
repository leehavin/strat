using Strat.Infrastructure.Extensions;
using Strat.Infrastructure.Models.User;
using Strat.Infrastructure.Services.Abstractions;
using Strat.Shared.CommonViewModels;
using Strat.Shared.Dialogs;
using Strat.Shared.Layout;
using Strat.Shared.Models;
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
        private readonly IStratDialogService _dialogService;
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

        /// <summary>
        /// 新增命令
        /// </summary>
        public DelegateCommand AddCommand { get; }

        /// <summary>
        /// 编辑命令
        /// </summary>
        public DelegateCommand<UserResponse> EditCommand { get; }

        /// <summary>
        /// 删除命令
        /// </summary>
        public DelegateCommand<UserResponse> DeleteCommand { get; }

        /// <summary>
        /// 批量删除命令
        /// </summary>
        public DelegateCommand BatchDeleteCommand { get; }

        /// <summary>
        /// 重置密码命令
        /// </summary>
        public DelegateCommand<UserResponse> ResetPasswordCommand { get; }

        /// <summary>
        /// 切换状态命令
        /// </summary>
        public DelegateCommand<UserResponse> ToggleStatusCommand { get; }

        #endregion

        #region Constructor

        public UserViewModel(IEventAggregator eventAggregator, IUserService userService, IStratDialogService dialogService) 
            : base(eventAggregator)
        {
            _userService = userService;
            _dialogService = dialogService;
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

            AddCommand = new DelegateCommand(ExecuteAddCommand);
            EditCommand = new DelegateCommand<UserResponse>(ExecuteEditCommand);
            DeleteCommand = new DelegateCommand<UserResponse>(ExecuteDeleteCommand);
            BatchDeleteCommand = new DelegateCommand(ExecuteBatchDeleteCommand);
            ResetPasswordCommand = new DelegateCommand<UserResponse>(ExecuteResetPasswordCommand);
            ToggleStatusCommand = new DelegateCommand<UserResponse>(ExecuteToggleStatusCommand);

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

        #region CRUD Operations (增删改操作)

        private void ExecuteAddCommand()
        {
            _dialogService.ShowDialog("UserEditDialog", null, async (result, vm) =>
            {
                if (result && vm is UserEditDialogViewModel editVm)
                {
                    var userEditModel = editVm.User;
                    if (userEditModel != null)
                    {
                        var input = new AddUserInput
                        {
                            Account = userEditModel.Account,
                            Name = userEditModel.Name,
                            Password = userEditModel.Password,
                            Telephone = userEditModel.Telephone ?? "",
                            Email = userEditModel.Email ?? "",
                            Status = userEditModel.Status,
                            OrganizationId = 0, // 暂未实现
                            RoleId = 0         // 暂未实现
                        };

                        var success = await _userService.AddAsync(input);
                        if (success)
                        {
                            _dialogService.ShowToast("新增用户成功", ToastType.Success);
                            await LoadUsersAsync();
                        }
                        else
                        {
                            _dialogService.ShowToast("新增用户失败", ToastType.Error);
                        }
                    }
                }
            });
        }

        private void ExecuteEditCommand(UserResponse user)
        {
            if (user == null) return;

            _dialogService.ShowDialog("UserEditDialog", user, async (result, vm) =>
            {
                if (result && vm is UserEditDialogViewModel editVm)
                {
                    var userEditModel = editVm.User;
                    if (userEditModel != null)
                    {
                        var input = new UpdateUserInput
                        {
                            Id = userEditModel.Id,
                            Account = userEditModel.Account,
                            Name = userEditModel.Name,
                            Telephone = userEditModel.Telephone ?? "",
                            Email = userEditModel.Email ?? "",
                            Status = userEditModel.Status,
                            OrganizationId = user.OrganizationId,
                            RoleId = user.RoleId
                        };

                        var success = await _userService.UpdateAsync(input);
                        if (success)
                        {
                            _dialogService.ShowToast("更新用户成功", ToastType.Success);
                            await LoadUsersAsync();
                        }
                        else
                        {
                            _dialogService.ShowToast("更新用户失败", ToastType.Error);
                        }
                    }
                }
            });
        }

        private async void ExecuteDeleteCommand(UserResponse user)
        {
            if (user == null) return;

            var confirm = await _dialogService.ShowConfirmAsync($"确定要删除用户 [{user.Name}] 吗？\n删除后不可恢复。");
            if (!confirm) return;

            try
            {
                var success = await _userService.DeleteAsync(user.Id);
                if (success)
                {
                    _dialogService.ShowToast("删除成功", Shared.Layout.ToastType.Success);
                    await LoadUsersAsync();
                }
                else
                {
                    _dialogService.ShowToast("删除失败", Shared.Layout.ToastType.Error);
                }
            }
            catch (Exception ex)
            {
                _dialogService.ShowToast($"删除出错: {ex.Message}", Shared.Layout.ToastType.Error);
            }
        }

        private async void ExecuteBatchDeleteCommand()
        {
            var selectedItems = UserPaginationModel?.Items.Where(x => x.IsSelected).ToList();
            if (selectedItems == null || !selectedItems.Any())
            {
                _dialogService.ShowToast("请先选择要删除的记录", Shared.Layout.ToastType.Warning);
                return;
            }

            var confirm = await _dialogService.ShowConfirmAsync($"确定要批量删除这 {selectedItems.Count} 条记录吗？");
            if (!confirm) return;

            try
            {
                var ids = selectedItems.Select(x => x.Id).ToList();
                var success = await _userService.BatchDeleteAsync(ids);
                if (success)
                {
                    _dialogService.ShowToast("批量删除成功", Shared.Layout.ToastType.Success);
                    await LoadUsersAsync();
                }
                else
                {
                    _dialogService.ShowToast("批量删除失败", Shared.Layout.ToastType.Error);
                }
            }
            catch (Exception ex)
            {
                _dialogService.ShowToast($"批量删除出错: {ex.Message}", Shared.Layout.ToastType.Error);
            }
        }

        private async void ExecuteResetPasswordCommand(UserResponse user)
        {
             if (user == null) return;
             
             // TODO: 打开重置密码对话框或直接重置
             _dialogService.ShowToast("功能开发中...", Shared.Layout.ToastType.Info);
        }

        private async void ExecuteToggleStatusCommand(UserResponse user)
        {
             if (user == null) return;
             
             // 状态取反：1 -> 0, 0 -> 1 (假设 1=启用, 0=禁用)
             int newStatus = user.Status == 1 ? 0 : 1;
             
             try
             {
                 var success = await _userService.ChangeStatusAsync(user.Id, newStatus);
                 if (success)
                 {
                     user.Status = newStatus; // 乐观更新 UI
                     _dialogService.ShowToast(newStatus == 1 ? "已启用" : "已禁用", Shared.Layout.ToastType.Success);
                 }
                 else
                 {
                     // 失败则回滚 UI 状态（如果需要）或重新加载
                     await LoadUsersAsync();
                     _dialogService.ShowToast("状态修改失败", Shared.Layout.ToastType.Error);
                 }
             }
             catch (Exception ex)
             {
                 _dialogService.ShowToast($"操作出错: {ex.Message}", Shared.Layout.ToastType.Error);
             }
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
