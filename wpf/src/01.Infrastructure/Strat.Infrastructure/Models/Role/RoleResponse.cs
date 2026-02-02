using System;

namespace Strat.Infrastructure.Models.Role
{
    public class RoleResponse
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreateTime { get; set; }
        public string? Remark { get; set; }
    }
}

