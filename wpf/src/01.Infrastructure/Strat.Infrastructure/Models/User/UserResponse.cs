using Prism.Mvvm;

namespace Strat.Infrastructure.Models.User;

public class UserResponse : BindableBase
{
    private bool _isSelected;
    public bool IsSelected
    {
        get => _isSelected;
        set => SetProperty(ref _isSelected, value);
    }

    public long Id { get; set; }
    public string? Remark { get; set; }
    public string Account { get; set; } = string.Empty;
    public string? Password { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Telephone { get; set; }
    public string? Email { get; set; }
    public byte[]? Avatar { get; set; }
    public long OrganizationId { get; set; }
    public string? OrganizationName { get; set; }
    public long RoleId { get; set; }
    public string? RoleName { get; set; }
    public int Status { get; set; }
    public DateTime? CreateTime { get; set; }
}
