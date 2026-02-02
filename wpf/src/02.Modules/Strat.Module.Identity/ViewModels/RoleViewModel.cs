using System;
using System.Collections.Generic;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Events;
using Strat.Infrastructure.Services.Abstractions;
using Strat.Infrastructure.Models.Role;
using Strat.Shared.Models;
using Strat.Shared.CommonViewModels;
using Strat.Infrastructure.Extensions;
using Mapster;
using System.Collections.ObjectModel;

namespace Strat.Module.Identity.ViewModels
{
    public class RoleViewModel : StratViewModelBase
    {
        private readonly IRoleService _roleService;

        public RoleViewModel(IEventAggregator eventAggregator, IRoleService roleService) : base(eventAggregator)
        {
            _roleService = roleService;
            _searchModel = new RoleSearchModel();
            _searchModel.PropertyChanged += (s, e) => 
            {
                if (e.PropertyName == nameof(PagedParams.PageIndex))
                {
                    ExecuteSearchCommand();
                }
            };
            
            ExecuteSearchCommand();
        }

        private RoleSearchModel _searchModel;
        public RoleSearchModel SearchModel
        {
            get => _searchModel;
            set => SetProperty(ref _searchModel, value);
        }

        private PagedViewModel<RoleResponse>? _rolePaginationModel;
        public PagedViewModel<RoleResponse>? RolePaginationModel
        {
            get => _rolePaginationModel;
            set => SetProperty(ref _rolePaginationModel, value);
        }

        private DelegateCommand? _searchCommand;
        public DelegateCommand SearchCommand => _searchCommand ??= new DelegateCommand(ExecuteSearchCommand);

        private async void ExecuteSearchCommand()
        {
            try
            {
                var input = SearchModel.Adapt<GetRolePagedRequest>();
                var result = await _roleService.GetPagedListAsync(input);
                RolePaginationModel = result.ToUIResult();
            }
            catch (Exception ex)
            {
                Strat.Shared.Logging.StratLogger.Error($"[Role] 查询角色列表失败: {ex.Message}");
                RolePaginationModel = new PagedViewModel<RoleResponse>();
            }
        }

        private DelegateCommand? _resetCommand;
        public DelegateCommand ResetCommand => _resetCommand ??= new DelegateCommand(() => {
            SearchModel = new RoleSearchModel();
            ExecuteSearchCommand();
        });
    }

    public class RoleSearchModel : PagedParams
    {
        private string? _name;
        public string? Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string? _code;
        public string? Code
        {
            get => _code;
            set => SetProperty(ref _code, value);
        }
    }
}

