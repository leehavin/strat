using Strat.System.Application.Contracts;
using Strat.System.Domain;
using Volo.Abp.Modularity;

namespace Strat.System.Application;

[DependsOn(
    typeof(StratSystemApplicationContractsModule),
    typeof(StratSystemDomainModule)
)]
public class StratSystemApplicationModule : AbpModule
{
}

