using Strat.Identity.Domain;
using Volo.Abp.Modularity;

namespace Strat.Identity.Application.Contracts;

[DependsOn(typeof(StratIdentityDomainModule))]
public class StratIdentityApplicationContractsModule : AbpModule
{
}

