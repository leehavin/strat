namespace Strat.Identity.Application.Contracts.Role.Dtos;

/// <summary>
/// 添加角色输入
/// </summary>
public class AddRoleRequest
{
    /// <summary>
    /// 角色名称
    /// </summary>
    [Required(ErrorMessage = "角色名称不能为空")]
    [MaxLength(50, ErrorMessage = "角色名称最大长度为50")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 角色编码
    /// </summary>
    [Required(ErrorMessage = "角色编码不能为空")]
    [MaxLength(50, ErrorMessage = "角色编码最大长度为50")]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// 角色描述
    /// </summary>
    [MaxLength(200, ErrorMessage = "角色描述最大长度为200")]
    public string? Description { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(500, ErrorMessage = "备注最大长度为500")]
    public string? Remark { get; set; }
}

