using Avalonia;
using Strat.Infrastructure.Models.Organization;
using Strat.Infrastructure.Services.Abstractions;
using Strat.Shared.CommonViewModels;
using Strat.Shared.Dialogs;
using Prism.Commands;
using System.Collections.ObjectModel;

namespace Strat.Module.System.ViewModels;

public class OrganizationManagementViewModel : StratViewModelBase
{
    private readonly IOrganizationService _orgService;
    private readonly IStratDialogService _dialogService;

    public OrganizationManagementViewModel(
        IEventAggregator eventAggregator,
        IOrganizationService orgService,
        IStratDialogService dialogService) : base(eventAggregator)
    {
        _orgService = orgService;
        _dialogService = dialogService;
        Title = "组织架构";
    }

    private ObservableCollection<OrganizationNode> _organizations = new();
    public ObservableCollection<OrganizationNode> Organizations
    {
        get => _organizations;
        set => SetProperty(ref _organizations, value);
    }

    public override async void OnNavigatedTo(NavigationContext navigationContext)
    {
        base.OnNavigatedTo(navigationContext);
        await LoadData();
    }

    private async Task LoadData()
    {
        try
        {
            IsBusy = true;
            var tree = await _orgService.GetTreeAsync();
            var nodes = BuildFlatTree(tree, 0);
            Organizations = new ObservableCollection<OrganizationNode>(nodes);
        }
        catch (Exception ex)
        {
            _dialogService.ShowToast($"加载组织数据失败: {ex.Message}", Shared.Layout.ToastType.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }

    private List<OrganizationNode> BuildFlatTree(List<OrganizationResponse> items, int level)
    {
        var result = new List<OrganizationNode>();
        foreach (var item in items)
        {
            var node = new OrganizationNode(item, level);
            result.Add(node);
            
            if (item.Children != null && item.Children.Count > 0)
            {
                result.AddRange(BuildFlatTree(item.Children, level + 1));
            }
        }
        return result;
    }

    private DelegateCommand? _addCommand;
    public DelegateCommand AddCommand => _addCommand ??= new DelegateCommand(ExecuteAdd);

    private DelegateCommand<OrganizationNode>? _editCommand;
    public DelegateCommand<OrganizationNode> EditCommand => _editCommand ??= new DelegateCommand<OrganizationNode>(ExecuteEdit);

    private DelegateCommand<OrganizationNode>? _deleteCommand;
    public DelegateCommand<OrganizationNode> DeleteCommand => _deleteCommand ??= new DelegateCommand<OrganizationNode>(ExecuteDelete);

    private void ExecuteAdd()
    {
        ShowEditDialog(null);
    }

    private void ExecuteEdit(OrganizationNode? node)
    {
        if (node != null)
        {
            ShowEditDialog(node.Data);
        }
    }

    private void ShowEditDialog(OrganizationResponse? org)
    {
        _dialogService.ShowDialog("OrganizationEditDialog", org, async (result, _) =>
        {
            if (result)
            {
                await LoadData();
            }
        });
    }

    private async void ExecuteDelete(OrganizationNode? node)
    {
        if (node == null) return;

        var result = await _dialogService.ShowConfirmAsync($"确定要删除组织 \"{node.Name}\" 吗？");
        if (result)
        {
            try
            {
                IsBusy = true;
                var success = await _orgService.DeleteAsync(node.Id);
                if (success)
                {
                    _dialogService.ShowToast("删除成功", Shared.Layout.ToastType.Success);
                    await LoadData();
                }
                else
                {
                    _dialogService.ShowToast("删除失败", Shared.Layout.ToastType.Error);
                }
            }
            catch (Exception ex)
            {
                _dialogService.ShowToast($"删除失败: {ex.Message}", Shared.Layout.ToastType.Error);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}

public class OrganizationNode : BindableBase
{
    public OrganizationNode(OrganizationResponse data, int level)
    {
        Data = data;
        Level = level;
    }

    public OrganizationResponse Data { get; }
    public int Level { get; }

    public Avalonia.Thickness IndentMargin => new(Level * 20, 0, 0, 0);

    // Proxy properties for DataGrid bindings
    public long Id => Data.Id;
    public string Name => Data.Name;
    public string Code => Data.Code;
    public string? Leader => Data.Leader;
    public string? Telephone => Data.Telephone;
    public int Sort => Data.Sort;
    public int Status => Data.Status;
    public DateTime CreateTime => Data.CreateTime;
}
