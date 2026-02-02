namespace Strat.Workflow.Application.Contracts.Definition.Dtos;

public class WfDefinitionResponse
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string DefinitionId { get; set; } = string.Empty;
    public string? WorkflowContent { get; set; }
    public string? DesignsContent { get; set; }
    public string? FormContent { get; set; }
    public int Version { get; set; }
    public bool IsLocked { get; set; }
    public string? Remark { get; set; }
    public DateTime CreateTime { get; set; }
}

