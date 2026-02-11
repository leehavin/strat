namespace Strat.Infrastructure.Models.Organization;

public class AddOrganizationInput
{
    public long ParentId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public int Sort { get; set; }
    public string? Leader { get; set; }
    public string? Telephone { get; set; }
    public string? Email { get; set; }
    public int Status { get; set; } = 1;
    public string? Remark { get; set; }
}

public class UpdateOrganizationInput : AddOrganizationInput
{
    public long Id { get; set; }
}
