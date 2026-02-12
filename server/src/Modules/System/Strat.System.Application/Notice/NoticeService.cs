using Strat.Identity.Domain.User;
using Strat.Infrastructure.Persistence;
using Strat.Shared.Abstractions;

namespace Strat.System.Application.Notice;

/// <summary>
/// 通知公告服务
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroupConst.System)]
[Authorize]
public class NoticeService(
    ISqlSugarClient db,
    IRepository<NoticeEntity> noticeRepository)
    : ApplicationService, INoticeService
{
    private readonly ISqlSugarClient _db = db;
    private readonly IRepository<NoticeEntity> _noticeRepository = noticeRepository;

    /// <summary>
    /// 分页查询
    /// </summary>
    public async Task<PagedList<NoticeResponse>> GetPagedListAsync(GetNoticePagedRequest input)
    {
        RefAsync<int> total = 0;
        var list = await _db.Queryable<NoticeEntity>()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Title), x => x.Title.Contains(input.Title!))
            .WhereIF(input.NoticeType.HasValue, x => x.NoticeType == input.NoticeType)
            .WhereIF(input.Level.HasValue, x => x.Level == input.Level)
            .LeftJoin<DictDataEntity>((n, dt) => n.NoticeType == dt.Id)
            .LeftJoin<DictDataEntity>((n, dt, dl) => n.Level == dl.Id)
            .Select((n, dt, dl) => new NoticeResponse
            {
                Id = n.Id,
                Title = n.Title,
                Content = n.Content,
                NoticeType = n.NoticeType,
                Level = n.Level,
                NoticeTypeName = dt.Name,
                LevelName = dl.Name,
                Remark = n.Remark,
                CreateTime = n.CreateTime
            })
            .OrderByDescending(n => n.CreateTime)
            .ToPageListAsync(input.PageIndex, input.PageSize, total);

        return list.ToPagedList(total, input.PageIndex, input.PageSize);
    }

    /// <summary>
    /// 获取单条记录
    /// </summary>
    public async Task<NoticeResponse> GetAsync(long id)
    {
        var entity = await _noticeRepository.GetByIdAsync(id)
            ?? throw BusinessException.Throw(ErrorTipsEnum.NoResult);
        return entity.Adapt<NoticeResponse>();
    }

    /// <summary>
    /// 添加通知公告
    /// </summary>
    public async Task<long> AddAsync(AddNoticeRequest input)
    {
        var entity = input.Adapt<NoticeEntity>();
        var noticeId = await _noticeRepository.InsertReturnSnowflakeIdAsync(entity);

        // 全员发送通知记录
        var users = await _db.Queryable<UserEntity>()
            .Where(x => x.Status == (int)UserStatusEnum.Normal)
            .Select(x => x.Id)
            .ToListAsync();

        if (users.Count > 0)
        {
            var records = users.Select(userId => new NoticeRecordEntity
            {
                NoticeId = noticeId,
                Receiver = userId
            }).ToList();

            await _db.Insertable(records).ExecuteReturnSnowflakeIdListAsync();
        }

        return noticeId;
    }

    /// <summary>
    /// 更新通知公告
    /// </summary>
    public async Task UpdateAsync(long id, AddNoticeRequest input)
    {
        var entity = await _noticeRepository.GetByIdAsync(id)
            ?? throw BusinessException.Throw(ErrorTipsEnum.NoResult);

        input.Adapt(entity);
        await _noticeRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除通知公告
    /// </summary>
    public async Task DeleteAsync(long id)
    {
        var entity = await _noticeRepository.GetByIdAsync(id)
            ?? throw BusinessException.Throw(ErrorTipsEnum.NoResult);

        // 先删除通知记录
        await _db.Deleteable<NoticeRecordEntity>()
            .Where(x => x.NoticeId == id)
            .ExecuteCommandAsync();

        // 再删除通知
        await _noticeRepository.DeleteAsync(entity);
    }

    /// <summary>
    /// 发送通知给指定用户
    /// </summary>
    public async Task SendAsync(long id, long[] userIds)
    {
        // 检查通知是否存在
        var exists = await _noticeRepository.Queryable().AnyAsync(x => x.Id == id);
        if (!exists)
        {
            throw BusinessException.Throw(ErrorTipsEnum.NoResult);
        }

        // 获取有效用户
        var validUsers = await _db.Queryable<UserEntity>()
            .Where(x => userIds.Contains(x.Id) && x.Status == (int)UserStatusEnum.Normal)
            .Select(x => x.Id)
            .ToListAsync();

        if (validUsers.Count > 0)
        {
            // 获取已存在的记录
            var existingRecords = await _db.Queryable<NoticeRecordEntity>()
                .Where(x => x.NoticeId == id && validUsers.Contains(x.Receiver))
                .Select(x => x.Receiver)
                .ToListAsync();

            // 只添加不存在的记录
            var newUserIds = validUsers.Except(existingRecords).ToList();
            if (newUserIds.Count > 0)
            {
                var records = newUserIds.Select(userId => new NoticeRecordEntity
                {
                    NoticeId = id,
                    Receiver = userId
                }).ToList();

                await _db.Insertable(records).ExecuteReturnSnowflakeIdListAsync();
            }
        }
    }
}

