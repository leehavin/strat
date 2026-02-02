using Strat.Shared;
using Volo.Abp.Modularity;

namespace Strat.Identity.Domain;

[DependsOn(typeof(StratSharedModule))]
public class StratIdentityDomainModule : AbpModule
{
}

