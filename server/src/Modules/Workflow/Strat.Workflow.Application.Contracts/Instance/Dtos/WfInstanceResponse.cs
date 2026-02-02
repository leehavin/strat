namespace Strat.Workflow.Application.Contracts.Instance.Dtos;

public class WfInstanceResponse
{
    public long PersistenceId { get; set; }
    public string InstanceId { get; set; } = string.Empty;
    public string WorkflowDefinitionId { get; set; } = string.Empty;
    public string? DefinitionName { get; set; }
    public int Status { get; set; }
    public string? StatusName => Status switch
    {
        0 => "运行中",
        1 => "已挂起",
        2 => "已完成",
        3 => "已终止",
        _ => "未知"
    };
    public string? Description { get; set; }
    public DateTime? CompleteTime { get; set; }
    public long CreateBy { get; set; }
    public string? CreateByName { get; set; }
    public DateTime CreateTime { get; set; }
    public string? Remark { get; set; }
}

