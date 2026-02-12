using Prism.Commands;
using Strat.Infrastructure.Models.Function;
using Strat.Infrastructure.Services.Abstractions;
using Strat.Shared.CommonViewModels;
using Strat.Shared.Dialogs;
using Strat.Shared.Layout;
using System.ComponentModel.DataAnnotations;

namespace Strat.Module.System.ViewModels;

public class FunctionEditDialogViewModel : StratValidateModel
{
    private readonly IFunctionService _functionService;
    private readonly IStratDialogService _dialogService;
    private bool _isEdit;

    public FunctionEditDialogViewModel(
        IFunctionService functionService,
        IStratDialogService dialogService)
    {
        _functionService = functionService;
        _dialogService = dialogService;
    }

    private FunctionEditModel _model = new();
    public FunctionEditModel Model
    {
        get => _model;
        set => SetProperty(ref _model, value);
    }

    public string Title => _isEdit ? "编辑功能" : "新增功能";

    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set => SetProperty(ref _isBusy, value);
    }

    public event Action<bool>? RequestClose;

    public void Initialize(object? parameters)
    {
        if (parameters is FunctionResponse org)
        {
            _isEdit = true;
            Model = new FunctionEditModel
            {
                Id = org.Id,
                ParentId = org.ParentId,
                Name = org.Name,
                Code = org.Code,
                Type = org.Type,
                Sort = org.Sort,
                Icon = org.Icon,
                Path = org.Path,
                Component = org.Component,
                Visible = org.Visible,
                Remark = org.Remark
            };
        }
        else
        {
            _isEdit = false;
            Model = new FunctionEditModel
            {
                Sort = 0,
                Visible = true,
                Type = 1 // Default to Menu
            };
        }
        RaisePropertyChanged(nameof(Title));
    }

    private DelegateCommand? _saveCommand;
    public DelegateCommand SaveCommand => _saveCommand ??= new DelegateCommand(ExecuteSave);

    private DelegateCommand? _cancelCommand;
    public DelegateCommand CancelCommand => _cancelCommand ??= new DelegateCommand(ExecuteCancel);

    private async void ExecuteSave()
    {
        if (!Model.Validate())
        {
            return;
        }

        try
        {
            IsBusy = true;
            bool success;
            if (_isEdit)
            {
                var input = new UpdateFunctionInput
                {
                    Id = Model.Id,
                    ParentId = Model.ParentId,
                    Name = Model.Name,
                    Code = Model.Code,
                    Type = Model.Type,
                    Sort = Model.Sort,
                    Icon = Model.Icon,
                    Path = Model.Path,
                    Component = Model.Component,
                    Visible = Model.Visible,
                    Status = 1, // Default enabled
                    Remark = Model.Remark
                };
                success = await _functionService.UpdateAsync(input);
            }
            else
            {
                var input = new AddFunctionInput
                {
                    ParentId = Model.ParentId,
                    Name = Model.Name,
                    Code = Model.Code,
                    Type = Model.Type,
                    Sort = Model.Sort,
                    Icon = Model.Icon,
                    Path = Model.Path,
                    Component = Model.Component,
                    Visible = Model.Visible,
                    Status = 1, // Default enabled
                    Remark = Model.Remark
                };
                var id = await _functionService.AddAsync(input);
                success = id > 0;
            }

            if (success)
            {
                _dialogService.ShowToast(_isEdit ? "更新成功" : "新增成功", ToastType.Success);
                RequestClose?.Invoke(true);
            }
            else
            {
                _dialogService.ShowToast(_isEdit ? "更新失败" : "新增失败", ToastType.Error);
            }
        }
        catch (Exception ex)
        {
            _dialogService.ShowToast($"操作失败: {ex.Message}", ToastType.Error);
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
}

public class FunctionEditModel : StratValidateModel
{
    public long Id { get; set; }
    public long? ParentId { get; set; }

    private string _name = string.Empty;
    [Required(ErrorMessage = "名称不能为空")]
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    private string _code = string.Empty;
    [Required(ErrorMessage = "编码不能为空")]
    public string Code
    {
        get => _code;
        set => SetProperty(ref _code, value);
    }

    private int _type;
    public int Type
    {
        get => _type;
        set => SetProperty(ref _type, value);
    }

    private int _sort;
    public int Sort
    {
        get => _sort;
        set => SetProperty(ref _sort, value);
    }

    private string? _icon;
    public string? Icon
    {
        get => _icon;
        set => SetProperty(ref _icon, value);
    }

    private string? _path;
    public string? Path
    {
        get => _path;
        set => SetProperty(ref _path, value);
    }

    private string? _component;
    public string? Component
    {
        get => _component;
        set => SetProperty(ref _component, value);
    }

    private bool _visible = true;
    public bool Visible
    {
        get => _visible;
        set => SetProperty(ref _visible, value);
    }

    private string? _remark;
    public string? Remark
    {
        get => _remark;
        set => SetProperty(ref _remark, value);
    }
}
