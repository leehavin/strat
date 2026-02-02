namespace Strat.System.Application.Contracts.Dict.Dtos;

/// <summary>
/// 字典分类输出
/// </summary>
public class DictCategoryResponse
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Remark { get; set; }
    public DateTime CreateTime { get; set; }
}

