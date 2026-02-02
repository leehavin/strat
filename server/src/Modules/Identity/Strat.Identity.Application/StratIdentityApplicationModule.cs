using Strat.Identity.Application.Contracts;
using Strat.Identity.Domain;
using Volo.Abp.Modularity;

namespace Strat.Identity.Application;

[DependsOn(
    typeof(StratIdentityApplicationContractsModule),
    typeof(StratIdentityDomainModule)
)]
public class StratIdentityApplicationModule : AbpModule
{
}

