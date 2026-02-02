namespace Strat.System.Application.Contracts.Dict.Dtos;

/// <summary>
/// 字典分页查询输入
/// </summary>
public class GetDictPagedRequest : PagedRequest
{
    /// <summary>
    /// 名称（模糊查询）
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 分类ID
    /// </summary>
    public long? CategoryId { get; set; }
}

