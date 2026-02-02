namespace Strat.Workflow.Domain.Instance;

/// <summary>
/// 工作流实例实体
/// </summary>
[SugarTable("WF_WORKFLOW")]
public class WfWorkflowEntity
{
    [SugarColumn(ColumnName = "PERSISTENCE_ID", IsPrimaryKey = true)]
    public long PersistenceId { get; set; }

    [SugarColumn(ColumnName = "INSTANCE_ID")]
    public string InstanceId { get; set; } = string.Empty;

    [SugarColumn(ColumnName = "WORKFLOW_DEFINITION_ID")]
    public string WorkflowDefinitionId { get; set; } = string.Empty;

    [SugarColumn(ColumnName = "VERSION")]
    public int Version { get; set; }

    [SugarColumn(ColumnName = "STATUS")]
    public int Status { get; set; }

    [SugarColumn(ColumnName = "DATA")]
    public string? Data { get; set; }

    [SugarColumn(ColumnName = "DESCRIPTION")]
    public string? Description { get; set; }

    [SugarColumn(ColumnName = "REFERENCE")]
    public string? Reference { get; set; }

    [SugarColumn(ColumnName = "NEXT_EXECUTION")]
    public long? NextExecution { get; set; }

    [SugarColumn(ColumnName = "COMPLETE_TIME")]
    public DateTime? CompleteTime { get; set; }

    [SugarColumn(ColumnName = "CREATE_BY")]
    public long CreateBy { get; set; }

    [SugarColumn(ColumnName = "CREATE_TIME")]
    public DateTime CreateTime { get; set; }

    [SugarColumn(ColumnName = "REMARK")]
    public string? Remark { get; set; }

    #region 导航属性

    [SugarColumn(IsIgnore = true)]
    public string? CreateByName { get; set; }

    [SugarColumn(IsIgnore = true)]
    public string? DefinitionName { get; set; }

    #endregion
}

