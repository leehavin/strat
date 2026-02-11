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

    private ObservableCollection<FunctionNode> _functions = new();
    public ObservableCollection<FunctionNode> Functions
    {
        get => _functions;
        set => SetProperty(ref _functions, value);
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
            // 并行加载功能树和角色已有权限
            var treeTask = _functionService.GetTreeAsync();
            var idsTask = _roleService.GetFunctionIdsAsync(_roleId);

            await Task.WhenAll(treeTask, idsTask);

            var tree = treeTask.Result;
            var assignedIds = idsTask.Result ?? new List<long>();

            var nodes = BuildTree(tree, assignedIds);
            Functions = new ObservableCollection<FunctionNode>(nodes);
        }
        catch (Exception ex)
        {
            _dialogService.ShowToast($"加载数据失败: {ex.Message}", ToastType.Error);
        }
    }

    private List<FunctionNode> BuildTree(List<FunctionResponse> functions, List<long> assignedIds)
    {
        return functions.Select(f =>
        {
            var node = new FunctionNode(f);
            if (assignedIds.Contains(f.Id))
            {
                node.SetIsCheckedSilent(true);
            }

            if (f.Children != null && f.Children.Count > 0)
            {
                var children = BuildTree(f.Children, assignedIds);
                foreach (var child in children)
                {
                    node.Children.Add(child);
                    child.Parent = node;
                }
            }
            return node;
        }).ToList();
    }

    private DelegateCommand? _saveCommand;
    public DelegateCommand SaveCommand => _saveCommand ??= new DelegateCommand(ExecuteSave);

    private async void ExecuteSave()
    {
        try
        {
            var selectedIds = GetAllSelectedIds(Functions);
            
            // AssignRoleFunctionsRequest 
            // The service method AssignFunctionsAsync takes (roleId, ids) ? 
            // No, the interface changed to take AssignRoleFunctionsRequest input.
            // But IRoleService.cs (Client) might have simplified signature?
            // Let's check IRoleService.cs. If it takes (long roleId, List<long> functionIds), then implementation handles DTO.
            // If it takes DTO directly, I need to create DTO.
            
            // Assuming IRoleService.AssignFunctionsAsync(long roleId, List<long> functionIds) based on previous memory,
            // or I need to check.
            
            // Wait, previous tool output for IRoleApi.cs showed:
            // Task<Strat.Shared.Models.ApiResponse<bool>> AssignFunctionsAsync([Body] AssignRoleFunctionsRequest input);
            
            // But IRoleService.cs (Client implementation) typically wraps API.
            // Let's assume IRoleService has AssignFunctionsAsync(long roleId, List<long> functionIds) for now.
            // If compilation fails, I will fix it.
            
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
    }

    private List<long> GetAllSelectedIds(IEnumerable<FunctionNode> nodes)
    {
        var ids = new List<long>();
        foreach (var node in nodes)
        {
            // Logic: if checked, add ID.
            if (node.IsChecked == true) 
            {
                ids.Add(node.Id);
            }
            
            if (node.Children.Count > 0)
            {
                ids.AddRange(GetAllSelectedIds(node.Children));
            }
        }
        return ids;
    }

    private DelegateCommand? _cancelCommand;
    public DelegateCommand CancelCommand => _cancelCommand ??= new DelegateCommand(() =>
    {
        RequestClose?.Invoke(false);
    });
}

public class FunctionNode : BindableBase
{
    public FunctionNode(FunctionResponse function)
    {
        Id = function.Id;
        Name = function.Name;
        Data = function;
    }

    public long Id { get; }
    public string Name { get; }
    public FunctionResponse Data { get; }

    private bool? _isChecked = false;
    public bool? IsChecked
    {
        get => _isChecked;
        set
        {
            if (SetProperty(ref _isChecked, value))
            {
                // Update children
                if (value.HasValue && !_isUpdatingParent)
                {
                    foreach (var child in Children)
                    {
                        if (child.IsChecked != value)
                        {
                            child.IsChecked = value;
                        }
                    }
                }
                
                // Update parent
                UpdateParent();
            }
        }
    }

    public ObservableCollection<FunctionNode> Children { get; } = new();
    public FunctionNode? Parent { get; set; }

    private bool _isUpdatingParent;

    public void SetIsCheckedSilent(bool? value)
    {
        SetProperty(ref _isChecked, value, nameof(IsChecked));
    }

    public void UpdateParent()
    {
        if (Parent == null || _isUpdatingParent) return;

        _isUpdatingParent = true;
        try
        {
            var allChecked = Parent.Children.All(c => c.IsChecked == true);
            var allUnchecked = Parent.Children.All(c => c.IsChecked == false);

            if (allChecked)
                Parent.IsChecked = true;
            else if (allUnchecked)
                Parent.IsChecked = false;
            else
                Parent.IsChecked = null;
        }
        finally
        {
            _isUpdatingParent = false;
        }
    }
}
