using Strat.Infrastructure.Models.Role;
using Strat.Infrastructure.Services.Abstractions;
using Strat.Shared.CommonViewModels;
using Strat.Shared.Dialogs;
using Prism.Commands;
using System.Collections.ObjectModel;

namespace Strat.Module.System.ViewModels
{
    public class RoleManagementViewModel : StratViewModelBase
    {
        private readonly IRoleService _roleService;
        private readonly IStratDialogService _dialogService;

        public RoleManagementViewModel(
            IEventAggregator eventAggregator, 
            IRoleService roleService,
            IStratDialogService dialogService) : base(eventAggregator)
        {
            _roleService = roleService;
            _dialogService = dialogService;
            Title = "角色管理";
        }

        private ObservableCollection<RoleResponse> _roles = new();
        public ObservableCollection<RoleResponse> Roles
        {
            get => _roles;
            set => SetProperty(ref _roles, value);
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
                   PageArgs_PageIndex = 1; 
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

        public override void OnLoaded()
        {
            base.OnLoaded();
            LoadData();
        }

        private DelegateCommand? _loadDataCommand;
        public DelegateCommand LoadDataCommand => _loadDataCommand ??= new DelegateCommand(LoadData);

        private DelegateCommand? _addCommand;
        public DelegateCommand AddCommand => _addCommand ??= new DelegateCommand(ExecuteAdd);

        private DelegateCommand<RoleResponse>? _editCommand;
        public DelegateCommand<RoleResponse> EditCommand => _editCommand ??= new DelegateCommand<RoleResponse>(ExecuteEdit);

        private DelegateCommand<RoleResponse>? _permissionCommand;
        public DelegateCommand<RoleResponse> PermissionCommand => _permissionCommand ??= new DelegateCommand<RoleResponse>(ExecutePermission);

        private async void LoadData()
        {
            await ExecuteAsync(async () =>
            {
                var request = new GetRolePagedRequest 
                { 
                    PageIndex = PageArgs_PageIndex, 
                    PageSize = PageArgs_PageSize,
                    Name = SearchKeyword,
                    Code = SearchKeyword
                };
                
                var result = await _roleService.GetPagedListAsync(request);
                if (result != null)
                {
                    Roles = new ObservableCollection<RoleResponse>(result.Items);
                    TotalCount = (int)result.Total;
                }
            });
        }

        private void ExecuteAdd()
        {
            _dialogService.ShowDialog("RoleEditDialog", null, (result, _) =>
            {
                if (result)
                {
                    _dialogService.ShowToast("新增成功", Strat.Shared.Layout.ToastType.Success);
                    LoadData();
                }
            });
        }

        private void ExecuteEdit(RoleResponse role)
        {
            _dialogService.ShowDialog("RoleEditDialog", role, (result, _) =>
            {
                if (result)
                {
                    _dialogService.ShowToast("编辑成功", Strat.Shared.Layout.ToastType.Success);
                    LoadData();
                }
            });
        }

        private void ExecutePermission(RoleResponse role)
        {
            var parameters = new DialogParameters { { "RoleId", role.Id } };
            _dialogService.ShowDialog("RolePermissionDialog", parameters, (result, _) =>
            {
                if (result)
                {
                    _dialogService.ShowToast("权限分配成功", Strat.Shared.Layout.ToastType.Success);
                }
            });
        }
    }
}