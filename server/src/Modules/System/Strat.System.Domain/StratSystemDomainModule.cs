using Strat.Shared;
using Volo.Abp.Modularity;

namespace Strat.System.Domain;

[DependsOn(typeof(StratSharedModule))]
public class StratSystemDomainModule : AbpModule
{
}

