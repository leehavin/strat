namespace Strat.System.Application.Contracts.Organization.Dtos;

/// <summary>
/// 添加组织架构输入
/// </summary>
public class AddOrganizationRequest
{
    [Required(ErrorMessage = "组织名称不能为空")]
    [MaxLength(100, ErrorMessage = "组织名称最大长度为100")]
    public string Name { get; set; } = string.Empty;

    public long? ParentId { get; set; }

    [MaxLength(20, ErrorMessage = "联系电话最大长度为20")]
    public string? Telephone { get; set; }

    [MaxLength(50, ErrorMessage = "负责人最大长度为50")]
    public string? Leader { get; set; }

    public int Sort { get; set; }

    [MaxLength(500, ErrorMessage = "备注最大长度为500")]
    public string? Remark { get; set; }
}

