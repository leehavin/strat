namespace Strat.System.Application.Contracts.Dict.Dtos;

/// <summary>
/// 字典数据输出
/// </summary>
public class DictDataResponse
{
    public long Id { get; set; }
    public long CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public int Sort { get; set; }
    public string? Remark { get; set; }
    public DateTime CreateTime { get; set; }
}

