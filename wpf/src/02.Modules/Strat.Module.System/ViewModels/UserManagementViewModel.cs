using Strat.Infrastructure.Models.User;
using Strat.Infrastructure.Services.Abstractions;
using Strat.Shared.CommonViewModels;
using Strat.Shared.Dialogs;
using System.Collections.ObjectModel;
using System.Linq;

namespace Strat.Module.System.ViewModels
{
    public class UserManagementViewModel : StratViewModelBase
    {
        private readonly IUserService _userService;
        private readonly IStratDialogService _dialogService;

        public UserManagementViewModel(
            IEventAggregator eventAggregator, 
            IUserService userService,
            IStratDialogService dialogService) : base(eventAggregator)
        {
            _userService = userService;
            _dialogService = dialogService;
            Title = "用户管理";
        }

        private ObservableCollection<UserResponse> _users = new();
        public ObservableCollection<UserResponse> Users
        {
            get => _users;
            set => SetProperty(ref _users, value);
        }

        private int _pageArgs_PageIndex = 1;
        public int PageArgs_PageIndex
        {
            get => _pageArgs_PageIndex;
            set
            {
                if (SetProperty(ref _pageArgs_PageIndex, value))
                {
                   LoadData();
                }
            }
        }

        private int _pageArgs_PageSize = 20;
        public int PageArgs_PageSize
        {
            get => _pageArgs_PageSize;
            set
            {
                if (SetProperty(ref _pageArgs_PageSize, value))
                {
                   PageArgs_PageIndex = 1; // 重置到第一页
                   LoadData();
                }
            }
        }

        private int _totalCount;
        public int TotalCount
        {
            get => _totalCount;
            set => SetProperty(ref _totalCount, value);
        }

        private string? _searchKeyword;
        public string? SearchKeyword
        {
            get => _searchKeyword;
            set => SetProperty(ref _searchKeyword, value);
        }

        private int _searchStatusIndex = 0;
        public int SearchStatusIndex
        {
            get => _searchStatusIndex;
            set => SetProperty(ref _searchStatusIndex, value);
        }

        public override void OnLoaded()
        {
            base.OnLoaded();
            LoadData();
        }

        private DelegateCommand? _loadDataCommand;
        public DelegateCommand LoadDataCommand => _loadDataCommand ??= new DelegateCommand(LoadData);

        private DelegateCommand? _searchCommand;
        public DelegateCommand SearchCommand => _searchCommand ??= new DelegateCommand(() =>
        {
            PageArgs_PageIndex = 1;
            LoadData();
        });

        private DelegateCommand? _resetCommand;
        public DelegateCommand ResetCommand => _resetCommand ??= new DelegateCommand(() =>
        {
            SearchKeyword = null;
            SearchStatusIndex = 0;
            PageArgs_PageIndex = 1;
            LoadData();
        });

        private DelegateCommand? _exportCommand;
        public DelegateCommand ExportCommand => _exportCommand ??= new DelegateCommand(() =>
        {
            _dialogService.ShowToast("导出功能暂未实现", Strat.Shared.Layout.ToastType.Info);
        });

        private DelegateCommand? _addCommand;
        public DelegateCommand AddCommand => _addCommand ??= new DelegateCommand(ExecuteAdd);

        private DelegateCommand? _batchDeleteCommand;
        public DelegateCommand BatchDeleteCommand => _batchDeleteCommand ??= new DelegateCommand(ExecuteBatchDelete);

        private DelegateCommand<UserResponse>? _editCommand;
        public DelegateCommand<UserResponse> EditCommand => _editCommand ??= new DelegateCommand<UserResponse>(ExecuteEdit);

        private DelegateCommand<UserResponse>? _deleteCommand;
        public DelegateCommand<UserResponse> DeleteCommand => _deleteCommand ??= new DelegateCommand<UserResponse>(ExecuteDelete);

        private async void LoadData()
        {
            await ExecuteAsync(async () =>
            {
                int? status = SearchStatusIndex switch
                {
                    1 => 1, // 正常
                    2 => 0, // 禁用
                    _ => null // 全部
                };

                var request = new GetPagedListRequest 
                { 
                    PageIndex = PageArgs_PageIndex, 
                    PageSize = PageArgs_PageSize,
                    Name = SearchKeyword,
                    Account = SearchKeyword,
                    Status = status
                };
                
                var result = await _userService.GetPagedListAsync(request);
                if (result != null)
                {
                    Users = new ObservableCollection<UserResponse>(result.Items);
                    TotalCount = (int)result.Total;
                }
            });
        }

        private void ExecuteAdd()
        {
            _dialogService.ShowDialog("UserEditDialog", null, (result, _) =>
            {
                if (result)
                {
                    _dialogService.ShowToast("新增成功", Strat.Shared.Layout.ToastType.Success);
                    LoadData();
                }
            });
        }

        private async void ExecuteBatchDelete()
        {
            var selectedUsers = Users.Where(u => u.IsSelected).ToList();
            if (selectedUsers.Count == 0)
            {
                _dialogService.ShowToast("请先选择要删除的用户", Strat.Shared.Layout.ToastType.Warning);
                return;
            }

            var confirm = await _dialogService.ShowConfirmAsync($"确定要删除选中的 {selectedUsers.Count} 个用户吗？", "删除确认");
            if (confirm)
            {
                await ExecuteAsync(async () =>
                {
                    var ids = selectedUsers.Select(u => u.Id).ToList();
                    var success = await _userService.BatchDeleteAsync(ids);
                    if (success)
                    {
                        _dialogService.ShowToast("删除成功", Strat.Shared.Layout.ToastType.Success);
                        LoadData();
                    }
                    else
                    {
                        _dialogService.ShowToast("删除失败", Strat.Shared.Layout.ToastType.Error);
                    }
                });
            }
        }

        private void ExecuteEdit(UserResponse user)
        {
            _dialogService.ShowDialog("UserEditDialog", user, (result, _) =>
            {
                if (result)
                {
                    _dialogService.ShowToast("编辑成功", Strat.Shared.Layout.ToastType.Success);
                    LoadData();
                }
            });
        }

        private async void ExecuteDelete(UserResponse user)
        {
            var confirm = await _dialogService.ShowConfirmAsync($"确定要删除用户 {user.Name} 吗？", "删除确认");
            if (confirm)
            {
                await ExecuteAsync(async () =>
                {
                    var success = await _userService.DeleteAsync(user.Id);
                    if (success)
                    {
                        _dialogService.ShowToast("删除成功", Strat.Shared.Layout.ToastType.Success);
                        LoadData();
                    }
                    else
                    {
                        _dialogService.ShowToast("删除失败", Strat.Shared.Layout.ToastType.Error);
                    }
                });
            }
        }
    }
}