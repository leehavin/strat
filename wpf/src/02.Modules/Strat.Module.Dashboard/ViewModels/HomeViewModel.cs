using Strat.Shared.CommonViewModels;

namespace Strat.Module.Dashboard.ViewModels
{
    public class HomeViewModel : StratViewModelBase
    {
        private string _welcomeMessage = "欢迎使用 STRAT 管理系统";

        public string WelcomeMessage
        {
            get => _welcomeMessage;
            set => SetProperty(ref _welcomeMessage, value);
        }

        public HomeViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
        }
    }
}
