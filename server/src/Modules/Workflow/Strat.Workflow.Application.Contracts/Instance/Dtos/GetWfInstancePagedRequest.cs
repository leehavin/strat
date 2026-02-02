namespace Strat.Workflow.Application.Contracts.Instance.Dtos;

public class GetWfInstancePagedRequest : PagedRequest
{
    public int? Status { get; set; }
    public string? WorkflowDefinitionId { get; set; }
}

