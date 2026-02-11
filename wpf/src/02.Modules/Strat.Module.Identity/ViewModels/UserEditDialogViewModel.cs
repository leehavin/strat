using Strat.Infrastructure.Models.User;

namespace Strat.Module.Identity.ViewModels
{
    public class UserEditDialogViewModel : BindableBase
    {
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

        public UserEditDialogViewModel()
        {
            SaveCommand = new DelegateCommand(OnSave);
            CancelCommand = new DelegateCommand(OnCancel);
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
                    Status = user.Status
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

        private void OnSave()
        {
            // 简单校验
            if (string.IsNullOrWhiteSpace(User.Account)) return;
            if (string.IsNullOrWhiteSpace(User.Name)) return;
            if (!IsEditMode && string.IsNullOrWhiteSpace(User.Password)) return;

            RequestClose?.Invoke(true);
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
    }
}
