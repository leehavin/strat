using Strat.System.Domain;
using Volo.Abp.Modularity;

namespace Strat.System.Application.Contracts;

[DependsOn(typeof(StratSystemDomainModule))]
public class StratSystemApplicationContractsModule : AbpModule
{
}

