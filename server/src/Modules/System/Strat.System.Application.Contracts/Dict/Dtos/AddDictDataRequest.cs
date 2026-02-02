namespace Strat.System.Application.Contracts.Dict.Dtos;

/// <summary>
/// 添加字典数据输入
/// </summary>
public class AddDictDataRequest
{
    [Required(ErrorMessage = "分类ID不能为空")]
    public long CategoryId { get; set; }

    [Required(ErrorMessage = "字典名称不能为空")]
    [MaxLength(50, ErrorMessage = "字典名称最大长度为50")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "字典编码不能为空")]
    [MaxLength(50, ErrorMessage = "字典编码最大长度为50")]
    public string Code { get; set; } = string.Empty;

    public int Sort { get; set; }

    [MaxLength(500, ErrorMessage = "备注最大长度为500")]
    public string? Remark { get; set; }
}

