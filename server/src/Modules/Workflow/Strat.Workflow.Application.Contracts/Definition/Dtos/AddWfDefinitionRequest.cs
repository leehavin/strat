namespace Strat.Workflow.Application.Contracts.Definition.Dtos;

public class AddWfDefinitionRequest
{
    [Required(ErrorMessage = "流程名称不能为空")]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public string? WorkflowContent { get; set; }
    public string? DesignsContent { get; set; }
    public string? FormContent { get; set; }

    [MaxLength(500)]
    public string? Remark { get; set; }
}

