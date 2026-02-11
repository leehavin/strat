using Strat.Infrastructure.Models.Auth;
using Strat.Infrastructure.Services.Abstractions;
using Strat.Module.Identity.Models;
using Strat.Shared.CommonViewModels;
using Strat.Shared.Dialogs;
using Strat.Shared.Logging;
using Strat.Shared.Services;

namespace Strat.Module.Identity.ViewModels
{
    public class LoginViewModel : StratViewModelBase
    {
        private readonly IAuthService _authService;
        private readonly IRegionManager _regionManager;
        private readonly IStratDialogService _dialogService;
        private readonly ILocalizationService _localizationService;
        private readonly IThemeService _themeService;

        public LoginViewModel(
            IEventAggregator eventAggregator, 
            IAuthService authService, 
            IRegionManager regionManager, 
            IStratDialogService dialogService,
            ILocalizationService localizationService,
            IThemeService themeService) : base(eventAggregator)
        {
            _authService = authService;
            _regionManager = regionManager;
            _dialogService = dialogService;
            _localizationService = localizationService;
            _themeService = themeService;
            
            LoginUser = new LoginModel { Account = "admin", Password = "123456" };
        }

        public IEnumerable<LanguageInfo> SupportedLanguages => _localizationService.SupportedLanguages;
        
        public LanguageInfo CurrentLanguage
        {
            get => _localizationService.CurrentLanguage;
            set
            {
                if (value != null && value.Key != _localizationService.CurrentLanguage.Key)
                {
                    _localizationService.SetLanguage(value.Key);
                    RaisePropertyChanged(nameof(CurrentLanguage));
                }
            }
        }

        public ThemeMode CurrentTheme => _themeService.CurrentMode;

        private DelegateCommand? _switchThemeCommand;
        public DelegateCommand SwitchThemeCommand => 
            _switchThemeCommand ??= new DelegateCommand(() => 
            {
                _themeService.ToggleTheme();
                RaisePropertyChanged(nameof(CurrentTheme));
            });

        private LoginModel _loginUser = null!;
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
