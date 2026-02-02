using Strat.Workflow.Application.Contracts;
using Strat.Workflow.Domain;
using Volo.Abp.Modularity;

namespace Strat.Workflow.Application;

[DependsOn(
    typeof(StratWorkflowApplicationContractsModule),
    typeof(StratWorkflowDomainModule)
)]
public class StratWorkflowApplicationModule : AbpModule
{
}

