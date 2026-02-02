using Strat.Shared.CommonViewModels;

namespace Strat.Infrastructure.Models.Role
{
    public class GetRolePagedRequest : PagedParams
    {
        public string? Name { get; set; }
        public string? Code { get; set; }
    }
}

