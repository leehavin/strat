using Strat.Shared.Abstractions;

namespace Strat.Workflow.Application.Instance;

/// <summary>
/// 工作流实例服务
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroupConst.Workflow)]
public class WfInstanceService(
    ISqlSugarClient db,
    ICurrentUser currentUser,
    IUserQueryService userQuery) : ApplicationService, IWfInstanceService
{
    private readonly ISqlSugarClient _db = db;
    private readonly ICurrentUser _currentUser = currentUser;
    private readonly IUserQueryService _userQuery = userQuery;

    /// <summary>
    /// 获取我发起的流程
    /// </summary>
    public async Task<PagedList<WfInstanceResponse>> GetMyInstancesAsync(GetWfInstancePagedRequest input)
    {
        RefAsync<int> total = 0;

        var list = await _db.Queryable<WfWorkflowEntity>()
            .LeftJoin<WfDefinitionEntity>((w, d) => w.WorkflowDefinitionId == d.DefinitionId)
            .Where(w => w.CreateBy == _currentUser.UserId)
            .WhereIF(input.Status.HasValue, w => w.Status == input.Status)
            .WhereIF(input.WorkflowDefinitionId.IsNotNullOrWhiteSpace(), w => w.WorkflowDefinitionId == input.WorkflowDefinitionId)
            .OrderByDescending(w => w.CreateTime)
            .Select((w, d) => new WfWorkflowEntity
            {
                PersistenceId = w.PersistenceId,
                InstanceId = w.InstanceId,
                WorkflowDefinitionId = w.WorkflowDefinitionId,
                DefinitionName = d.Name,
                Status = w.Status,
                Description = w.Description,
                CompleteTime = w.CompleteTime,
                CreateBy = w.CreateBy,
                CreateTime = w.CreateTime,
                Remark = w.Remark
            })
            .ToPageListAsync(input.PageIndex, input.PageSize, total);

        var items = list.Adapt<List<WfInstanceResponse>>();

        // 填充创建人名称
        foreach (var item in items)
        {
            var user = await _userQuery.GetBasicInfoAsync(item.CreateBy);
            item.CreateByName = user?.Name;
        }

        return PagedList<WfInstanceResponse>.Create(items, total, input.PageIndex, input.PageSize);
    }

    /// <summary>
    /// 获取待办任务
    /// </summary>
    public async Task<PagedList<WfInstanceResponse>> GetPendingTasksAsync(GetWfInstancePagedRequest input)
    {
        // TODO: 实现待办任务查询，需要结合工作流引擎
        RefAsync<int> total = 0;
        var list = await _db.Queryable<WfWorkflowEntity>()
            .Where(w => w.Status == 0) // 运行中
            .OrderByDescending(w => w.CreateTime)
            .ToPageListAsync(input.PageIndex, input.PageSize, total);

        var items = list.Adapt<List<WfInstanceResponse>>();
        return PagedList<WfInstanceResponse>.Create(items, total, input.PageIndex, input.PageSize);
    }

    public async Task<WfInstanceResponse> GetAsync(long id)
    {
        var entity = await _db.Queryable<WfWorkflowEntity>()
            .LeftJoin<WfDefinitionEntity>((w, d) => w.WorkflowDefinitionId == d.DefinitionId)
            .Where(w => w.PersistenceId == id)
            .Select((w, d) => new WfWorkflowEntity
            {
                PersistenceId = w.PersistenceId,
                InstanceId = w.InstanceId,
                WorkflowDefinitionId = w.WorkflowDefinitionId,
                DefinitionName = d.Name,
                Status = w.Status,
                Description = w.Description,
                CompleteTime = w.CompleteTime,
                CreateBy = w.CreateBy,
                CreateTime = w.CreateTime,
                Remark = w.Remark
            })
            .FirstAsync();

        if (entity == null)
            throw BusinessException.Throw(ErrorTipsEnum.NoResult);

        var output = entity.Adapt<WfInstanceResponse>();
        var user = await _userQuery.GetBasicInfoAsync(entity.CreateBy);
        output.CreateByName = user?.Name;

        return output;
    }

    public async Task TerminateAsync(long id)
    {
        var entity = await _db.Queryable<WfWorkflowEntity>().FirstAsync(x => x.PersistenceId == id);
        if (entity == null)
            throw BusinessException.Throw(ErrorTipsEnum.NoResult);

        if (entity.Status != 0)
            throw BusinessException.Throw("只能终止运行中的流程");

        entity.Status = 3; // 已终止
        entity.Remark = "手动终止";

        await _db.Updateable(entity).UpdateColumns(x => new { x.Status, x.Remark }).ExecuteCommandAsync();
    }
}

