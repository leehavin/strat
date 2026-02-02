namespace Strat.System.Application.Contracts.Dict.Dtos;

/// <summary>
/// 添加字典分类输入
/// </summary>
public class AddDictCategoryRequest
{
    [Required(ErrorMessage = "分类名称不能为空")]
    [MaxLength(50, ErrorMessage = "分类名称最大长度为50")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "分类编码不能为空")]
    [MaxLength(50, ErrorMessage = "分类编码最大长度为50")]
    public string Code { get; set; } = string.Empty;

    [MaxLength(500, ErrorMessage = "备注最大长度为500")]
    public string? Remark { get; set; }
}

