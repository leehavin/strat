using Prism.Mvvm;

namespace Strat.Infrastructure.Models.Organization;

public class OrganizationResponse : BindableBase
{
    public long Id { get; set; }
    public long ParentId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public int Sort { get; set; }
    public string? Leader { get; set; }
    public string? Telephone { get; set; }
    public string? Email { get; set; }
    public int Status { get; set; }
    public string? Remark { get; set; }
    public DateTime CreateTime { get; set; }

    public List<OrganizationResponse>? Children { get; set; }
}
