using System.ComponentModel.DataAnnotations;

namespace Strat.Identity.Application.Contracts.Function.Dtos;

/// <summary>
/// 功能添加输入
/// </summary>
public class AddFunctionRequest
{
    /// <summary>
    /// 功能名称
    /// </summary>
    [Required(ErrorMessage = "功能名称不能为空")]
    [MaxLength(50, ErrorMessage = "功能名称最大长度为50")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 功能编码
    /// </summary>
    [Required(ErrorMessage = "功能编码不能为空")]
    [MaxLength(100, ErrorMessage = "功能编码最大长度为100")]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// 父级ID
    /// </summary>
    public long? ParentId { get; set; }

    /// <summary>
    /// 类型（0:目录 1:菜单 2:按钮）
    /// </summary>
    public int Type { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 图标
    /// </summary>
    [MaxLength(100, ErrorMessage = "图标最大长度为100")]
    public string? Icon { get; set; }

    /// <summary>
    /// 路由路径
    /// </summary>
    [MaxLength(200, ErrorMessage = "路由路径最大长度为200")]
    public string? Path { get; set; }

    /// <summary>
    /// 组件路径
    /// </summary>
    [MaxLength(200, ErrorMessage = "组件路径最大长度为200")]
    public string? Component { get; set; }

    /// <summary>
    /// 是否可见
    /// </summary>
    public bool Visible { get; set; } = true;

    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(500, ErrorMessage = "备注最大长度为500")]
    public string? Remark { get; set; }
}

