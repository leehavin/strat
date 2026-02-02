using Strat.Shared.Models;
using Strat.System.Application.Contracts.Notice.Dtos;
using Volo.Abp.Application.Services;

namespace Strat.System.Application.Contracts.Notice;

/// <summary>
/// 通知公告服务接口
/// </summary>
public interface INoticeService : IApplicationService
{
    /// <summary>
    /// 分页查询
    /// </summary>
    Task<PagedList<NoticeResponse>> GetPagedListAsync(GetNoticePagedRequest input);

    /// <summary>
    /// 获取单条记录
    /// </summary>
    Task<NoticeResponse> GetAsync(long id);

    /// <summary>
    /// 添加通知公告
    /// </summary>
    Task<long> AddAsync(AddNoticeRequest input);

    /// <summary>
    /// 更新通知公告
    /// </summary>
    Task UpdateAsync(long id, AddNoticeRequest input);

    /// <summary>
    /// 删除通知公告
    /// </summary>
    Task DeleteAsync(long id);

    /// <summary>
    /// 发送通知给指定用户
    /// </summary>
    Task SendAsync(long id, long[] userIds);
}

