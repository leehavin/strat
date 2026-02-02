using Prism.Commands;
using Prism.Mvvm;
using Prism.Events;
using Prism.Navigation.Regions;
using Strat.Infrastructure.Models.Auth;
using Strat.Infrastructure.Services.Abstractions;
using Strat.Shared.CommonViewModels;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Strat.Module.Identity.Models;
using Strat.Shared.I18n;
using Strat.Shared.Dialogs;
using Strat.Shared.Logging;

namespace Strat.Module.Identity.ViewModels
{
    public class LoginViewModel : StratViewModelBase
    {
        private readonly IAuthService _authService;
        private readonly IRegionManager _regionManager;
        private readonly IStratDialogService _dialogService;

        public LoginViewModel(IEventAggregator eventAggregator, IAuthService authService, IRegionManager regionManager, IStratDialogService dialogService) : base(eventAggregator)
        {
            _authService = authService;
            _regionManager = regionManager;
            _dialogService = dialogService;
            LoginUser = new LoginModel { Account = "admin", Password = "123456" };

            // 初始化语言列表
            Languages = new ObservableCollection<LanguageItem>(LanguageManager.Instance.SupportedLanguages);

            _selectedLanguage = Languages.FirstOrDefault(x => x.Key == LanguageManager.Instance.CurrentCulture);
        }

        public ObservableCollection<LanguageItem> Languages { get; }

        private LanguageItem? _selectedLanguage;
        public LanguageItem? SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                if (SetProperty(ref _selectedLanguage, value) && value != null)
                {
                    LanguageManager.Instance.SwitchLanguage(value.Key);
                }
            }
        }

        private LoginModel _loginUser;
        public LoginModel LoginUser
        {
            get => _loginUser;
            set => SetProperty(ref _loginUser, value);
        }

        private bool _showPassword;
        public bool ShowPassword
        {
            get => _showPassword;
            set => SetProperty(ref _showPassword, value);
        }

        private DelegateCommand? _togglePasswordVisibilityCommand;
        public DelegateCommand TogglePasswordVisibilityCommand => 
            _togglePasswordVisibilityCommand ??= new DelegateCommand(() => ShowPassword = !ShowPassword);

        private DelegateCommand? _loginCommand;
        public DelegateCommand LoginCommand => _loginCommand ??= new DelegateCommand(ExecuteLoginCommand, CanExecuteLogin)
            .ObservesProperty(() => IsBusy);

        private bool CanExecuteLogin()
        {
            return !IsBusy && LoginUser?.Validate() == true;
        }

        private async void ExecuteLoginCommand()
        {
            await ExecuteAsync(async () =>
            {
                var input = new LoginRequest { Account = LoginUser.Account, Password = LoginUser.Password };
                var result = await _authService.LoginAsync(input);

                if (result != null)
                {
                    StratLogger.Information($"[Login] 用户 {LoginUser.Account} 登录成功");
                    _regionManager.RequestNavigate("mainRegion", "Main", navigationResult =>
                    {
                        if (navigationResult.Success)
                        {
                            StratLogger.Information("[Login] 导航到 Main 成功");
                        }
                        else
                        {
                            StratLogger.Error($"[Login] 导航到 Main 失败: {navigationResult.Exception?.Message}");
                        }
                    });
                }
            });
        }
    }
}
