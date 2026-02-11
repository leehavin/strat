using Strat.Infrastructure.Models.Organization;
using Strat.Infrastructure.Services.Abstractions;
using Strat.Shared.Dialogs;
using Strat.Shared.Layout;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace Strat.Module.System.ViewModels;

public class OrganizationEditDialogViewModel : BindableBase
{
    private readonly IOrganizationService _orgService;
    private readonly IStratDialogService _dialogService;

    public OrganizationEditDialogViewModel(
        IOrganizationService orgService,
        IStratDialogService dialogService)
    {
        _orgService = orgService;
        _dialogService = dialogService;
    }

    private bool _isEditMode;
    public bool IsEditMode
    {
        get => _isEditMode;
        set => SetProperty(ref _isEditMode, value);
    }

    private string _title = "新增组织";
    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    private OrganizationEditModel _organization = new();
    public OrganizationEditModel Organization
    {
        get => _organization;
        set => SetProperty(ref _organization, value);
    }

    private ObservableCollection<OrganizationResponse> _parentOptions = new();
    public ObservableCollection<OrganizationResponse> ParentOptions
    {
        get => _parentOptions;
        set => SetProperty(ref _parentOptions, value);
    }

    public event Action<bool>? RequestClose;

    public void Initialize(OrganizationResponse? org)
    {
        LoadParents();

        if (org != null)
        {
            IsEditMode = true;
            Title = "编辑组织";
            Organization = new OrganizationEditModel
            {
                Id = org.Id,
                ParentId = org.ParentId,
                Name = org.Name,
                Code = org.Code,
                Sort = org.Sort,
                Leader = org.Leader,
                Telephone = org.Telephone,
                Email = org.Email,
                Status = org.Status,
                Remark = org.Remark
            };
        }
        else
        {
            IsEditMode = false;
            Title = "新增组织";
            Organization = new OrganizationEditModel { Status = 1, Sort = 1 };
        }
    }

    private async void LoadParents()
    {
        try
        {
            // For parent selection, we usually need a flattened list or tree. 
            // Here assuming simple list for ComboBox, or flatten tree manually.
            var tree = await _orgService.GetTreeAsync();
            var flat = BuildFlatList(tree, 0);
            ParentOptions = new ObservableCollection<OrganizationResponse>(flat);
        }
        catch (Exception ex)
        {
            _dialogService.ShowToast($"加载上级组织失败: {ex.Message}", ToastType.Error);
        }
    }

    private List<OrganizationResponse> BuildFlatList(List<OrganizationResponse> items, int level)
    {
        var result = new List<OrganizationResponse>();
        foreach (var item in items)
        {
            // Indent name for display in ComboBox
            var displayItem = new OrganizationResponse
            {
                Id = item.Id,
                Name = new string(' ', level * 2) + item.Name, // Visual indentation
                ParentId = item.ParentId
            };
            result.Add(displayItem);

            if (item.Children != null && item.Children.Count > 0)
            {
                result.AddRange(BuildFlatList(item.Children, level + 1));
            }
        }
        return result;
    }

    private DelegateCommand? _saveCommand;
    public DelegateCommand SaveCommand => _saveCommand ??= new DelegateCommand(ExecuteSave);

    private async void ExecuteSave()
    {
        if (string.IsNullOrWhiteSpace(Organization.Name))
        {
            _dialogService.ShowToast("请输入组织名称", ToastType.Warning);
            return;
        }

        try
        {
            bool success;
            if (IsEditMode)
            {
                var input = new UpdateOrganizationInput
                {
                    Id = Organization.Id,
                    ParentId = Organization.ParentId,
                    Name = Organization.Name,
                    Code = Organization.Code,
                    Sort = Organization.Sort,
                    Leader = Organization.Leader,
                    Telephone = Organization.Telephone,
                    Email = Organization.Email,
                    Status = Organization.Status,
                    Remark = Organization.Remark
                };
                success = await _orgService.UpdateAsync(input);
            }
            else
            {
                var input = new AddOrganizationInput
                {
                    ParentId = Organization.ParentId,
                    Name = Organization.Name,
                    Code = Organization.Code,
                    Sort = Organization.Sort,
                    Leader = Organization.Leader,
                    Telephone = Organization.Telephone,
                    Email = Organization.Email,
                    Status = Organization.Status,
                    Remark = Organization.Remark
                };
                success = await _orgService.AddAsync(input);
            }

            if (success)
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

    private DelegateCommand? _cancelCommand;
    public DelegateCommand CancelCommand => _cancelCommand ??= new DelegateCommand(() =>
    {
        RequestClose?.Invoke(false);
    });
}

public class OrganizationEditModel : BindableBase
{
    public long Id { get; set; }

    private long _parentId;
    public long ParentId
    {
        get => _parentId;
        set => SetProperty(ref _parentId, value);
    }

    private string _name = string.Empty;
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    private string _code = string.Empty;
    public string Code
    {
        get => _code;
        set => SetProperty(ref _code, value);
    }

    private int _sort;
    public int Sort
    {
        get => _sort;
        set => SetProperty(ref _sort, value);
    }

    private string? _leader;
    public string? Leader
    {
        get => _leader;
        set => SetProperty(ref _leader, value);
    }

    private string? _telephone;
    public string? Telephone
    {
        get => _telephone;
        set => SetProperty(ref _telephone, value);
    }

    private string? _email;
    public string? Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
    }

    private int _status;
    public int Status
    {
        get => _status;
        set => SetProperty(ref _status, value);
    }

    private string? _remark;
    public string? Remark
    {
        get => _remark;
        set => SetProperty(ref _remark, value);
    }
}
