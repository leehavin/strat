namespace Strat.Infrastructure.Models.Role;

public class AssignRoleFunctionsRequest
{
    public long RoleId { get; set; }
    public List<long> FunctionIds { get; set; } = new();
}
