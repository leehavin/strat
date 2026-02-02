namespace Strat.System.Application.Contracts.Organization.Dtos;

/// <summary>
/// 组织架构输出
/// </summary>
public class OrganizationResponse
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public long? ParentId { get; set; }
    public string? Telephone { get; set; }
    public string? Leader { get; set; }
    public int Sort { get; set; }
    public string? Remark { get; set; }
    public DateTime CreateTime { get; set; }
    public List<OrganizationResponse>? Children { get; set; }
}

