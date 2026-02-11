using Strat.Infrastructure.Models.User;
using Strat.Infrastructure.Models.Role;
using Strat.Infrastructure.Models.Organization;
using Prism.Mvvm;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using Strat.Infrastructure.Services.Abstractions;
using Strat.Shared.Dialogs;
using System.Collections.Generic;

namespace Strat.Module.System.ViewModels
{
    public class UserEditDialogViewModel : BindableBase
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IOrganizationService _orgService;
        private readonly IStratDialogService _dialogService;

        public string Title => IsEditMode ? "编辑用户" : "新增用户";

        public event Action<bool>? RequestClose;

        private bool _isEditMode;
        public bool IsEditMode
        {
            get => _isEditMode;
            set
            {
                SetProperty(ref _isEditMode, value);
                RaisePropertyChanged(nameof(Title));
            }
        }

        private ObservableCollection<RoleResponse> _roles = new();
        public ObservableCollection<RoleResponse> Roles
        {
            get => _roles;
            set => SetProperty(ref _roles, value);
        }

        private UserEditModel _user = new();
        public UserEditModel User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }
        
        // 用于 ToggleSwitch 绑定 (Status 1=true, 0=false)
        public bool IsEnabled
        {
            get => User.Status == 1;
            set
            {
                User.Status = value ? 1 : 0;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand SaveCommand { get; }
        public DelegateCommand CancelCommand { get; }

        private ObservableCollection<OrganizationResponse> _organizations = new();
        public ObservableCollection<OrganizationResponse> Organizations
        {
            get => _organizations;
            set => SetProperty(ref _organizations, value);
        }

        public UserEditDialogViewModel(
            IUserService userService, 
            IRoleService roleService,
            IOrganizationService orgService,
            IStratDialogService dialogService)
        {
            _userService = userService;
            _roleService = roleService;
            _orgService = orgService;
            _dialogService = dialogService;
            SaveCommand = new DelegateCommand(OnSave);
            CancelCommand = new DelegateCommand(OnCancel);
            
            LoadRoles();
            LoadOrganizations();
        }

        private async void LoadOrganizations()
        {
            try
            {
                var tree = await _orgService.GetTreeAsync();
                var flat = BuildFlatList(tree, 0);
                Organizations = new ObservableCollection<OrganizationResponse>(flat);
            }
            catch (Exception ex)
            {
                _dialogService.ShowToast($"加载组织列表失败: {ex.Message}", Strat.Shared.Layout.ToastType.Error);
            }
        }

        private List<OrganizationResponse> BuildFlatList(List<OrganizationResponse> items, int level)
        {
            var result = new List<OrganizationResponse>();
            foreach (var item in items)
            {
                var displayItem = new OrganizationResponse
                {
                    Id = item.Id,
                    Name = new string(' ', level * 2) + item.Name,
                    ParentId = item.ParentId
                };
                result.Add(displayItem);

                if (item.Children != null && item.Children.Count > 0)
                {
                    result.AddRange(BuildFlatList(item.Children, level + 1));
                }
            }
            return result;
        }

        private async void LoadRoles()
        {
            try
            {
                var result = await _roleService.GetPagedListAsync(new GetRolePagedRequest 
                { 
                    PageIndex = 1, 
                    PageSize = 100 
                });
                if (result != null && result.Items != null)
                {
                    Roles = new ObservableCollection<RoleResponse>(result.Items);
                }
            }
            catch (Exception ex)
            {
                _dialogService.ShowToast($"加载角色列表失败: {ex.Message}", Strat.Shared.Layout.ToastType.Error);
            }
        }

        public void Initialize(UserResponse? user)
        {
            if (user != null)
            {
                IsEditMode = true;
                User = new UserEditModel
                {
                    Id = user.Id,
                    Account = user.Account,
                    Name = user.Name,
                    Telephone = user.Telephone,
                    Email = user.Email,
                    Status = user.Status,
                    RoleId = user.RoleId,
                    OrganizationId = user.OrganizationId
                };
                RaisePropertyChanged(nameof(IsEnabled));
            }
            else
            {
                IsEditMode = false;
                User = new UserEditModel { Status = 1 };
                RaisePropertyChanged(nameof(IsEnabled));
            }
        }

        private async void OnSave()
        {
            // 简单校验
            if (string.IsNullOrWhiteSpace(User.Account)) return;
            if (string.IsNullOrWhiteSpace(User.Name)) return;
            if (!IsEditMode && string.IsNullOrWhiteSpace(User.Password)) return;

            bool success;
            if (IsEditMode)
            {
                var input = new UpdateUserInput
                {
                    Id = User.Id,
                    Account = User.Account,
                    Name = User.Name,
                    Telephone = User.Telephone ?? "",
                    Email = User.Email ?? "",
                    Status = User.Status,
                    RoleId = User.RoleId,
                    OrganizationId = User.OrganizationId
                };
                success = await _userService.UpdateAsync(input);
            }
            else
            {
                var input = new AddUserInput
                {
                    Account = User.Account,
                    Password = User.Password,
                    Name = User.Name,
                    Telephone = User.Telephone ?? "",
                    Email = User.Email ?? "",
                    Status = User.Status,
                    RoleId = User.RoleId,
                    OrganizationId = User.OrganizationId
                };
                success = await _userService.AddAsync(input);
            }

            if (success)
            {
                RequestClose?.Invoke(true);
            }
            else
            {
                _dialogService.ShowToast("保存失败", Strat.Shared.Layout.ToastType.Error);
            }
        }

        private void OnCancel()
        {
            RequestClose?.Invoke(false);
        }
    }

    public class UserEditModel : BindableBase
    {
        public long Id { get; set; }
        
        private string _account = string.Empty;
        public string Account { get => _account; set => SetProperty(ref _account, value); }

        private string _name = string.Empty;
        public string Name { get => _name; set => SetProperty(ref _name, value); }
        
        private string _password = string.Empty;
        public string Password { get => _password; set => SetProperty(ref _password, value); }

        private string? _telephone;
        public string? Telephone { get => _telephone; set => SetProperty(ref _telephone, value); }

        private string? _email;
        public string? Email { get => _email; set => SetProperty(ref _email, value); }

        private int _status = 1;
        public int Status { get => _status; set => SetProperty(ref _status, value); }
        
        private long _roleId;
        public long RoleId { get => _roleId; set => SetProperty(ref _roleId, value); }

        private long _organizationId;
        public long OrganizationId { get => _organizationId; set => SetProperty(ref _organizationId, value); }
    }
}