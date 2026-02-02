using Strat.Workflow.Domain;
using Volo.Abp.Modularity;

namespace Strat.Workflow.Application.Contracts;

[DependsOn(typeof(StratWorkflowDomainModule))]
public class StratWorkflowApplicationContractsModule : AbpModule
{
}

