using Strat.Infrastructure.Models.User;
using Strat.Infrastructure.Services.Abstractions;
using Strat.Shared.CommonViewModels;
using System.Collections.ObjectModel;

namespace Strat.Module.System.ViewModels
{
    public class UserManagementViewModel : StratViewModelBase
    {
        private readonly IUserService _userService;

        public UserManagementViewModel(IEventAggregator eventAggregator, IUserService userService) : base(eventAggregator)
        {
            _userService = userService;
            Title = "用户管理";
        }

        private ObservableCollection<UserResponse> _users = new();
        public ObservableCollection<UserResponse> Users
        {
            get => _users;
            set => SetProperty(ref _users, value);
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
                   PageArgs_PageIndex = 1; // 重置到第一页
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

        public override void OnLoaded()
        {
            base.OnLoaded();
            LoadData();
        }

        private async void LoadData()
        {
            await ExecuteAsync(async () =>
            {
                var request = new GetPagedListRequest 
                { 
                    PageIndex = PageArgs_PageIndex, 
                    PageSize = PageArgs_PageSize 
                };
                
                var result = await _userService.GetPagedListAsync(request);
                if (result != null)
                {
                    Users = new ObservableCollection<UserResponse>(result.Items);
                    TotalCount = (int)result.Total;
                }
            });
        }
    }
}
