using Mapster;
using Strat.Infrastructure.Extensions;
using Strat.Infrastructure.Models.Role;
using Strat.Infrastructure.Services.Abstractions;
using Strat.Shared.CommonViewModels;
using Strat.Shared.Models;

namespace Strat.Module.Identity.ViewModels
{
    public class RoleViewModel : StratViewModelBase
    {
        private readonly IRoleService _roleService;
        private readonly Shared.Dialogs.IStratDialogService _dialogService;
        private CancellationTokenSource? _searchCts;

        public RoleViewModel(
            IEventAggregator eventAggregator, 
            IRoleService roleService,
            Shared.Dialogs.IStratDialogService dialogService) : base(eventAggregator)
        {
            _roleService = roleService;
            _dialogService = dialogService;
            _searchModel = new RoleSearchModel();
            
            _searchModel.PropertyChanged += (s, e) => 
            {
                if (e.PropertyName != nameof(PagedParams.PageIndex))
                {
                    SearchModel.PageIndex = 1;
                }
                ExecuteSearchCommand();
            };
            
            LoadRolesAsync();
        }

        private RoleSearchModel _searchModel;
        public RoleSearchModel SearchModel
        {
            get => _searchModel;
            set => SetProperty(ref _searchModel, value);
        }

        private PagedViewModel<RoleResponse>? _rolePaginationModel;
        public PagedViewModel<RoleResponse>? RolePaginationModel
        {
            get => _rolePaginationModel;
            set => SetProperty(ref _rolePaginationModel, value);
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        private DelegateCommand? _searchCommand;
        public DelegateCommand SearchCommand => _searchCommand ??= new DelegateCommand(ExecuteSearchCommand);

        private DelegateCommand? _resetCommand;
        public DelegateCommand ResetCommand => _resetCommand ??= new DelegateCommand(ExecuteResetCommand);

        private DelegateCommand? _addCommand;
        public DelegateCommand AddCommand => _addCommand ??= new DelegateCommand(ExecuteAddCommand);

        private DelegateCommand<RoleResponse>? _editCommand;
        public DelegateCommand<RoleResponse> EditCommand => _editCommand ??= new DelegateCommand<RoleResponse>(ExecuteEditCommand);

        private DelegateCommand<RoleResponse>? _deleteCommand;
        public DelegateCommand<RoleResponse> DeleteCommand => _deleteCommand ??= new DelegateCommand<RoleResponse>(ExecuteDeleteCommand);

        private DelegateCommand? _batchDeleteCommand;
        public DelegateCommand BatchDeleteCommand => _batchDeleteCommand ??= new DelegateCommand(ExecuteBatchDeleteCommand);

        private DelegateCommand<RoleResponse>? _toggleStatusCommand;
        public DelegateCommand<RoleResponse> ToggleStatusCommand => _toggleStatusCommand ??= new DelegateCommand<RoleResponse>(ExecuteToggleStatusCommand);

        private void ExecuteSearchCommand()
        {
            _searchCts?.Cancel();
            _searchCts = new CancellationTokenSource();
            var token = _searchCts.Token;

            Task.Delay(500, token).ContinueWith(t =>
            {
                if (!t.IsCanceled)
                {
                    Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(LoadRolesAsync);
                }
            }, token);
        }

        private void ExecuteResetCommand()
        {
            SearchModel = new RoleSearchModel();
            LoadRolesAsync();
        }

        private async Task LoadRolesAsync()
        {
            if (IsLoading) return;
            IsLoading = true;
            try
            {
                var input = SearchModel.Adapt<GetRolePagedRequest>();
                var result = await _roleService.GetPagedListAsync(input);
                RolePaginationModel = result.ToUIResult();
            }
            catch (Exception ex)
            {
                _dialogService.ShowToast($"加载角色列表失败: {ex.Message}", Shared.Layout.ToastType.Error);
                RolePaginationModel = new PagedViewModel<RoleResponse>();
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void ExecuteAddCommand()
        {
            _dialogService.ShowDialog("RoleEditDialog", null, async (result, vm) =>
            {
                if (result) await LoadRolesAsync();
            });
        }

        private void ExecuteEditCommand(RoleResponse role)
        {
            if (role == null) return;
            _dialogService.ShowDialog("RoleEditDialog", role, async (result, vm) =>
            {
                if (result) await LoadRolesAsync();
            });
        }

        private async void ExecuteDeleteCommand(RoleResponse role)
        {
            if (role == null) return;
            var confirm = await _dialogService.ShowConfirmAsync($"确定要删除角色 [{role.Name}] 吗？");
            if (confirm)
            {
                var success = await _roleService.DeleteAsync(role.Id);
                if (success)
                {
                    _dialogService.ShowToast("删除成功", Shared.Layout.ToastType.Success);
                    await LoadRolesAsync();
                }
            }
        }

        private async void ExecuteBatchDeleteCommand()
        {
            var selectedIds = RolePaginationModel?.Items.Where(x => x.IsSelected).Select(x => x.Id).ToList();
            if (selectedIds == null || !selectedIds.Any())
            {
                _dialogService.ShowToast("请选择要删除的角色", Shared.Layout.ToastType.Warning);
                return;
            }

            var confirm = await _dialogService.ShowConfirmAsync($"确定要删除选中的 {selectedIds.Count} 个角色吗？");
            if (confirm)
            {
                var success = await _roleService.BatchDeleteAsync(selectedIds);
                if (success)
                {
                    _dialogService.ShowToast("批量删除成功", Shared.Layout.ToastType.Success);
                    await LoadRolesAsync();
                }
            }
        }

        private async void ExecuteToggleStatusCommand(RoleResponse role)
        {
            if (role == null) return;
            int newStatus = role.Status == 1 ? 0 : 1;
            var success = await _roleService.ChangeStatusAsync(role.Id, newStatus);
            if (success)
            {
                role.Status = newStatus;
                _dialogService.ShowToast("状态更新成功", Shared.Layout.ToastType.Success);
            }
        }
    }

    public class RoleSearchModel : PagedParams
    {
        private string? _name;
        public string? Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string? _code;
        public string? Code
        {
            get => _code;
            set => SetProperty(ref _code, value);
        }
    }
}

