using Avalonia;
using Strat.Infrastructure.Models.Function;
using Strat.Infrastructure.Services.Abstractions;
using Strat.Shared.CommonViewModels;
using Strat.Shared.Dialogs;
using Prism.Commands;
using System.Collections.ObjectModel;

namespace Strat.Module.System.ViewModels;

public class FunctionManagementViewModel : StratViewModelBase
{
    private readonly IFunctionService _functionService;
    private readonly IStratDialogService _dialogService;

    public FunctionManagementViewModel(
        IEventAggregator eventAggregator,
        IFunctionService functionService,
        IStratDialogService dialogService) : base(eventAggregator)
    {
        _functionService = functionService;
        _dialogService = dialogService;
        Title = "功能管理";
    }

    private ObservableCollection<FunctionNode> _functions = new();
    public ObservableCollection<FunctionNode> Functions
    {
        get => _functions;
        set => SetProperty(ref _functions, value);
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
            var tree = await _functionService.GetTreeAsync();
            var nodes = BuildFlatTree(tree, 0);
            Functions = new ObservableCollection<FunctionNode>(nodes);
        }
        catch (Exception ex)
        {
            _dialogService.ShowToast($"加载功能数据失败: {ex.Message}", Shared.Layout.ToastType.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }

    private List<FunctionNode> BuildFlatTree(List<FunctionResponse> items, int level)
    {
        var result = new List<FunctionNode>();
        if (items == null) return result;

        foreach (var item in items)
        {
            var node = new FunctionNode(item, level);
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

    private DelegateCommand<FunctionNode>? _editCommand;
    public DelegateCommand<FunctionNode> EditCommand => _editCommand ??= new DelegateCommand<FunctionNode>(ExecuteEdit);

    private DelegateCommand<FunctionNode>? _deleteCommand;
    public DelegateCommand<FunctionNode> DeleteCommand => _deleteCommand ??= new DelegateCommand<FunctionNode>(ExecuteDelete);

    private void ExecuteAdd()
    {
        ShowEditDialog(null);
    }

    private void ExecuteEdit(FunctionNode? node)
    {
        if (node != null)
        {
            ShowEditDialog(node.Data);
        }
    }

    private void ShowEditDialog(FunctionResponse? func)
    {
        // Pass the entire tree for parent selection if needed, or just the item
        // For simplicity, we'll just pass the item. Parent selection might need a separate service call or tree passed in.
        // Actually, let's just pass the item.
        
        _dialogService.ShowDialog("FunctionEditDialog", func, async (result, _) =>
        {
            if (result)
            {
                await LoadData();
            }
        });
    }

    private async void ExecuteDelete(FunctionNode? node)
    {
        if (node == null) return;

        var result = await _dialogService.ShowConfirmAsync($"确定要删除功能 \"{node.Name}\" 吗？这将同时删除所有子功能。");
        if (result)
        {
            try
            {
                IsBusy = true;
                var success = await _functionService.DeleteAsync(node.Id);
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

public class FunctionNode : BindableBase
{
    public FunctionNode(FunctionResponse data, int level)
    {
        Data = data;
        Level = level;
    }

    public FunctionResponse Data { get; }
    public int Level { get; }

    public Avalonia.Thickness IndentMargin => new(Level * 20, 0, 0, 0);

    // Proxy properties
    public long Id => Data.Id;
    public string Name => Data.Name;
    public string Code => Data.Code;
    public int Type => Data.Type; // 0:Directory, 1:Menu, 2:Button
    public int Sort => Data.Sort;
    public string? Icon => Data.Icon;
    public string? Path => Data.Path;
    public string? Component => Data.Component;
    public bool Visible => Data.Visible;
    public bool IsDirectory => Type == 0;
    public bool IsMenu => Type == 1;
    public bool IsButton => Type == 2;
    public string TypeName => Type switch
    {
        0 => "目录",
        1 => "菜单",
        2 => "按钮",
        _ => "未知"
    };
}
