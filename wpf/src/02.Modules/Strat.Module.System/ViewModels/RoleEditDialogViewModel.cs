using Mapster;
using Strat.Infrastructure.Models.Auth;
using Strat.Infrastructure.Models.Role;
using Strat.Infrastructure.Services.Abstractions;
using Prism.Mvvm;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Strat.Module.System.ViewModels
{
    public class RoleEditDialogViewModel : BindableBase
    {
        private readonly IRoleService _roleService;
        private readonly IAuthService _authService;
        private readonly Shared.Dialogs.IStratDialogService _dialogService;

        public event Action<bool>? RequestClose;

        public RoleEditDialogViewModel(
            IRoleService roleService,
            IAuthService authService,
            Shared.Dialogs.IStratDialogService dialogService)
        {
            _roleService = roleService;
            _authService = authService;
            _dialogService = dialogService;

            SaveCommand = new DelegateCommand(ExecuteSaveCommand);
            CancelCommand = new DelegateCommand(() => RequestClose?.Invoke(false));
            
            _role = new RoleResponse { Status = 1 };
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        private RoleResponse _role;
        public RoleResponse Role
        {
            get => _role;
            set => SetProperty(ref _role, value);
        }

        private bool _isEditMode;
        public bool IsEditMode
        {
            get => _isEditMode;
            set => SetProperty(ref _isEditMode, value);
        }

        private ObservableCollection<PermissionTreeNode> _permissionTree = new ();
        public ObservableCollection<PermissionTreeNode> PermissionTree
        {
            get => _permissionTree;
            set => SetProperty(ref _permissionTree, value);
        }

        public DelegateCommand SaveCommand { get; }
        public DelegateCommand CancelCommand { get; }

        public async void Initialize(object? parameters)
        {
            // 加载权限树
            await LoadPermissionTreeAsync();

            if (parameters is RoleResponse role)
            {
                IsEditMode = true;
                Role = role.Adapt<RoleResponse>();
                // TODO: 如果有接口获取角色的权限ID列表，需在此调用并勾选树
                // 暂时假设 RoleResponse 中包含权限信息，或者需要额外请求
            }
            else
            {
                IsEditMode = false;
                Role = new RoleResponse { Status = 1 };
            }
        }

        private async Task LoadPermissionTreeAsync()
        {
            try
            {
                var routers = await _authService.GetRoutersAsync();
                var nodes = routers.Select(x => CreateNode(x)).ToList();
                PermissionTree = new ObservableCollection<PermissionTreeNode>(nodes);
            }
            catch (Exception ex)
            {
                _dialogService.ShowToast($"加载权限树失败: {ex.Message}", Shared.Layout.ToastType.Error);
            }
        }

        private PermissionTreeNode CreateNode(GetRoutersResponse router)
        {
            var node = new PermissionTreeNode
            {
                Id = router.Id,
                Name = router.Name,
                Code = router.Code
            };

            if (router.Children != null && router.Children.Any())
            {
                foreach (var child in router.Children)
                {
                    node.Children.Add(CreateNode(child));
                }
            }

            return node;
        }

        private async void ExecuteSaveCommand()
        {
            if (IsBusy) return;

            // 校验
            if (!Validate()) return;

            try
            {
                IsBusy = true;

                // 获取所有勾选的权限ID
                var permissionIds = new List<long>();
                CollectSelectedIds(PermissionTree, permissionIds);

                bool success;
                if (IsEditMode)
                {
                    var input = Role.Adapt<UpdateRoleInput>();
                    input.PermissionIds = permissionIds;
                    success = await _roleService.UpdateAsync(input);
                }
                else
                {
                    var input = Role.Adapt<AddRoleInput>();
                    input.PermissionIds = permissionIds;
                    success = await _roleService.AddAsync(input);
                }

                if (success)
                {
                    _dialogService.ShowToast(IsEditMode ? "更新角色成功" : "新增角色成功", Shared.Layout.ToastType.Success);
                    RequestClose?.Invoke(true);
                }
                else
                {
                    _dialogService.ShowToast(IsEditMode ? "更新角色失败" : "新增角色失败", Shared.Layout.ToastType.Error);
                }
            }
            catch (Exception ex)
            {
                _dialogService.ShowToast($"操作失败: {ex.Message}", Shared.Layout.ToastType.Error);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool Validate()
        {
            if (string.IsNullOrWhiteSpace(Role.Name))
            {
                _dialogService.ShowToast("请输入角色名称", Shared.Layout.ToastType.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(Role.Code))
            {
                _dialogService.ShowToast("请输入角色编码", Shared.Layout.ToastType.Warning);
                return false;
            }
            return true;
        }

        private void CollectSelectedIds(IEnumerable<PermissionTreeNode> nodes, List<long> ids)
        {
            foreach (var node in nodes)
            {
                if (node.IsChecked == true)
                {
                    ids.Add(node.Id);
                }
                CollectSelectedIds(node.Children, ids);
            }
        }
    }

    public class PermissionTreeNode : BindableBase
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;

        private bool? _isChecked = false;
        public bool? IsChecked
        {
            get => _isChecked;
            set 
            {
                if (SetProperty(ref _isChecked, value))
                {
                    UpdateChildren(value);
                    // TODO: UpdateParent() logic if needed for partial selection
                }
            }
        }

        public ObservableCollection<PermissionTreeNode> Children { get; } = new ();

        private void UpdateChildren(bool? value)
        {
            if (value == null) return;
            foreach (var child in Children)
            {
                child.IsChecked = value;
            }
        }
    }
}