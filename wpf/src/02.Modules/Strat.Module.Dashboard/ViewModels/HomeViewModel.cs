using Strat.Shared.CommonViewModels;
using System.Collections.ObjectModel;

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

        public ObservableCollection<int> ChartData { get; } = new();

        public HomeViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
            for (int i = 0; i < 12; i++)
            {
                ChartData.Add(i);
            }
        }
    }
}
