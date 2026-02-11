using System.Collections.Generic;

namespace Strat.Infrastructure.Models.Role
{
    public class AddRoleInput
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Status { get; set; }
        public List<long> PermissionIds { get; set; } = new List<long>();
    }

    public class UpdateRoleInput : AddRoleInput
    {
        public long Id { get; set; }
    }
}
