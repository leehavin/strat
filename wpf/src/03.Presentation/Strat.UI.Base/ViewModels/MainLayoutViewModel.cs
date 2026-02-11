using Strat.Shared.CommonViewModels;
using System.Collections.ObjectModel;

namespace Strat.UI.Base.ViewModels
{
    public class MainLayoutViewModel : StratViewModelBase
    {
        private readonly Prism.Navigation.Regions.IRegionManager _regionManager;

        public MainLayoutViewModel(IEventAggregator eventAggregator, Prism.Navigation.Regions.IRegionManager regionManager) : base(eventAggregator)
        {
            _regionManager = regionManager;
            Title = "Strat Admin";
            InitMenus();
        }

        public override void OnNavigatedTo(Prism.Navigation.Regions.NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            // 默认导航到 Home
            _regionManager.RequestNavigate("ContentRegion", "Home");
        }

        private bool _isPaneOpen = true;
        public bool IsPaneOpen
        {
            get => _isPaneOpen;
            set => SetProperty(ref _isPaneOpen, value);
        }

        public ObservableCollection<MenuItem> MenuItems { get; } = new();

        private void InitMenus()
        {
            MenuItems.Add(new MenuItem { Name = "Dashboard", Icon = "Home", ViewName = "Home" });
            
            var sysMenu = new MenuItem { Name = "System", Icon = "Setting" };
            sysMenu.Items.Add(new MenuItem { Name = "Users", Icon = "User", ViewName = "UserManagementView" });
            sysMenu.Items.Add(new MenuItem { Name = "Roles", Icon = "Lock", ViewName = "RoleManagementView" });
            
            MenuItems.Add(sysMenu);
        }
    }

    public class MenuItem
    {
        public string Name { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string? ViewName { get; set; }
        public ObservableCollection<MenuItem> Items { get; } = new();
    }
}
