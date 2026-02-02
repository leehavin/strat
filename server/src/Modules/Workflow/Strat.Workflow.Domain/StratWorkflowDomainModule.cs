using Strat.Shared;
using Volo.Abp.Modularity;

namespace Strat.Workflow.Domain;

[DependsOn(typeof(StratSharedModule))]
public class StratWorkflowDomainModule : AbpModule
{
}

