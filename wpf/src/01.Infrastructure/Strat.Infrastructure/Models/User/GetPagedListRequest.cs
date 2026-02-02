using Strat.Shared.CommonViewModels;

namespace Strat.Infrastructure.Models.User;

public class GetPagedListRequest : PagedParams
{
    public string? Account { get; set; }
    public string? Name { get; set; }
    public string? Telephone { get; set; }
    public string? Email { get; set; }
    public int? Status { get; set; }
}
