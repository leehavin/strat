namespace Strat.Workflow.Domain.Definition;

/// <summary>
/// 流程定义实体
/// </summary>
[SugarTable("WF_DEFINITION")]
public class WfDefinitionEntity
{
    [SugarColumn(ColumnName = "ID", IsPrimaryKey = true)]
    public long Id { get; set; }

    [SugarColumn(ColumnName = "NAME")]
    public string Name { get; set; } = string.Empty;

    [SugarColumn(ColumnName = "DEFINITION_ID")]
    public string DefinitionId { get; set; } = string.Empty;

    [SugarColumn(ColumnName = "WORKFLOW_CONTENT")]
    public string? WorkflowContent { get; set; }

    [SugarColumn(ColumnName = "DESIGNS_CONTENT")]
    public string? DesignsContent { get; set; }

    [SugarColumn(ColumnName = "FORM_CONTENT")]
    public string? FormContent { get; set; }

    [SugarColumn(ColumnName = "VERSION")]
    public int Version { get; set; } = 1;

    [SugarColumn(ColumnName = "IS_LOCKED")]
    public bool IsLocked { get; set; }

    [SugarColumn(ColumnName = "IS_DELETED")]
    public bool IsDeleted { get; set; }

    [SugarColumn(ColumnName = "CREATE_BY")]
    public long CreateBy { get; set; }

    [SugarColumn(ColumnName = "CREATE_TIME")]
    public DateTime CreateTime { get; set; }

    [SugarColumn(ColumnName = "UPDATE_BY")]
    public long? UpdateBy { get; set; }

    [SugarColumn(ColumnName = "UPDATE_TIME")]
    public DateTime? UpdateTime { get; set; }

    [SugarColumn(ColumnName = "REMARK")]
    public string? Remark { get; set; }
}

