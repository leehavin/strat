using Prism.Commands;
using Prism.Mvvm;
using Strat.Infrastructure.Models.Function;
using Strat.Infrastructure.Services.Abstractions;
using Strat.Shared.Dialogs;
using Strat.Shared.Layout;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Strat.Module.System.ViewModels;

public class RolePermissionDialogViewModel : BindableBase
{
    private readonly IFunctionService _functionService;
    private readonly IRoleService _roleService;
    private readonly IStratDialogService _dialogService;
    private long _roleId;

    public RolePermissionDialogViewModel(
        IFunctionService functionService,
        IRoleService roleService,
        IStratDialogService dialogService)
    {
        _functionService = functionService;
        _roleService = roleService;
        _dialogService = dialogService;
    }

    private ObservableCollection<CheckableFunctionNode> _functions = new();
    public ObservableCollection<CheckableFunctionNode> Functions
    {
        get => _functions;
        set => SetProperty(ref _functions, value);
    }

    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set => SetProperty(ref _isBusy, value);
    }

    public string Title => "分配权限";

    public event Action<bool>? RequestClose;

    public void Initialize(object? parameters)
    {
        if (parameters == null) return;

        try
        {
            // Handle DialogParameters (IEnumerable<KeyValuePair<string, object>>) or IDictionary
            if (parameters is IEnumerable<KeyValuePair<string, object>> dic)
            {
                var kvp = dic.FirstOrDefault(x => x.Key == "RoleId");
                if (!kvp.Equals(default(KeyValuePair<string, object>)))
                {
                    if (kvp.Value is long id) _roleId = id;
                    else if (kvp.Value is int i) _roleId = i;
                    LoadData();
                }
            }
            else if (parameters is global::System.Collections.IDictionary dictionary)
            {
                if (dictionary.Contains("RoleId"))
                {
                    var val = dictionary["RoleId"];
                    if (val is long id) _roleId = id;
                    else if (val is int i) _roleId = i;
                    LoadData();
                }
            }
        }
        catch (Exception ex)
        {
            _dialogService.ShowToast($"初始化参数失败: {ex.Message}", ToastType.Error);
        }
    }

    private async void LoadData()
    {
        try
        {
            IsBusy = true;
            // 并行加载功能树和角色已有权限
            var treeTask = _functionService.GetTreeAsync();
            var idsTask = _roleService.GetFunctionIdsAsync(_roleId);

            await Task.WhenAll(treeTask, idsTask);

            var tree = treeTask.Result;
            var assignedIds = idsTask.Result ?? new List<long>();

            // Build tree nodes
            var nodes = new List<CheckableFunctionNode>();
            foreach (var item in tree)
            {
                nodes.Add(BuildNode(item, assignedIds));
            }
            Functions = new ObservableCollection<CheckableFunctionNode>(nodes);
        }
        catch (Exception ex)
        {
            _dialogService.ShowToast($"加载数据失败: {ex.Message}", ToastType.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }

    private CheckableFunctionNode BuildNode(FunctionResponse item, List<long> assignedIds)
    {
        var node = new CheckableFunctionNode(item);
        
        // Check if assigned
        if (assignedIds.Contains(item.Id))
        {
            node.IsChecked = true;
        }

        if (item.Children != null && item.Children.Any())
        {
            foreach (var child in item.Children)
            {
                var childNode = BuildNode(child, assignedIds);
                childNode.Parent = node;
                node.Children.Add(childNode);
            }
        }
        
        return node;
    }

    private DelegateCommand? _saveCommand;
    public DelegateCommand SaveCommand => _saveCommand ??= new DelegateCommand(ExecuteSave);

    private DelegateCommand? _cancelCommand;
    public DelegateCommand CancelCommand => _cancelCommand ??= new DelegateCommand(ExecuteCancel);

    private async void ExecuteSave()
    {
        try
        {
            IsBusy = true;
            var selectedIds = GetAllSelectedIds(Functions);
            var result = await _roleService.AssignFunctionsAsync(_roleId, selectedIds);
            
            if (result)
            {
                _dialogService.ShowToast("保存成功", ToastType.Success);
                RequestClose?.Invoke(true);
            }
            else
            {
                _dialogService.ShowToast("保存失败", ToastType.Error);
            }
        }
        catch (Exception ex)
        {
            _dialogService.ShowToast($"保存失败: {ex.Message}", ToastType.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void ExecuteCancel()
    {
        RequestClose?.Invoke(false);
    }

    private List<long> GetAllSelectedIds(IEnumerable<CheckableFunctionNode> nodes)
    {
        var ids = new List<long>();
        foreach (var node in nodes)
        {
            if (node.IsChecked == true)
            {
                ids.Add(node.Id);
            }
            
            if (node.Children.Count > 0)
            {
                ids.AddRange(GetAllSelectedIds(node.Children));
            }
        }
        return ids.Distinct().ToList();
    }
}

public class CheckableFunctionNode : BindableBase
{
    public CheckableFunctionNode(FunctionResponse data)
    {
        Data = data;
    }

    public FunctionResponse Data { get; }
    public long Id => Data.Id;
    public string Name => Data.Name;

    private bool? _isChecked = false;
    public bool? IsChecked
    {
        get => _isChecked;
        set
        {
            if (SetProperty(ref _isChecked, value))
            {
                UpdateChildren(value);
                UpdateParent();
            }
        }
    }

    public CheckableFunctionNode? Parent { get; set; }
    public ObservableCollection<CheckableFunctionNode> Children { get; } = new();

    private void UpdateChildren(bool? value)
    {
        if (value == null) return;
        foreach (var child in Children)
        {
            // Avoid triggering parent update loop
            child._isChecked = value;
            child.RaisePropertyChanged(nameof(IsChecked));
            child.UpdateChildren(value);
        }
    }

    private void UpdateParent()
    {
        if (Parent == null) return;

        var allChecked = Parent.Children.All(x => x.IsChecked == true);
        var allUnchecked = Parent.Children.All(x => x.IsChecked == false);

        if (allChecked)
            Parent.SetIsCheckedSilent(true);
        else if (allUnchecked)
            Parent.SetIsCheckedSilent(false);
        else
            Parent.SetIsCheckedSilent(null);
            
        Parent.UpdateParent();
    }

    public void SetIsCheckedSilent(bool? value)
    {
        _isChecked = value;
        RaisePropertyChanged(nameof(IsChecked));
    }
}
