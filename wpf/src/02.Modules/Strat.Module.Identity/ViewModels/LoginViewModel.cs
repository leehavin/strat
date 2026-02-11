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
                    
                    // 手动保存一次 Token (保底，如果 RefitHandler 没拦截到)
                    // 虽然 RefitHandler 会处理，但登录接口通常在 Body 中也会返回 Token
                    // 如果 result.AccessToken 存在，可以保存
                    
                    _regionManager.RequestNavigate("mainRegion", "MainLayoutView", navigationResult =>
                    {
                        if (navigationResult.Success == true)
                        {
                            StratLogger.Information("[Login] 导航到主布局成功");
                        }
                        else
                        {
                            StratLogger.Error($"[Login] 导航到主布局失败: {navigationResult.Exception?.Message}");
                        }
                    });
                }
                else
                {
                    await _dialogService.ShowErrorAsync("登录失败，请检查用户名和密码", "登录提示");
                }
            });
        }
    }
}
