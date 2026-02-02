using System.ComponentModel.DataAnnotations;

namespace Strat.System.Application.Contracts.Notice.Dtos;

/// <summary>
/// 添加通知公告输入
/// </summary>
public class AddNoticeRequest
{
    /// <summary>
    /// 标题
    /// </summary>
    [Required(ErrorMessage = "标题不能为空")]
    [MaxLength(100, ErrorMessage = "标题最大长度为100")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 内容
    /// </summary>
    [Required(ErrorMessage = "内容不能为空")]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 通知类型（字典ID）
    /// </summary>
    public long NoticeType { get; set; }

    /// <summary>
    /// 级别（字典ID）
    /// </summary>
    public long Level { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(500, ErrorMessage = "备注最大长度为500")]
    public string? Remark { get; set; }
}

