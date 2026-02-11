using System;

namespace Strat.Infrastructure.Models.User
{
    public class AddUserInput
    {
        public string Account { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public long OrganizationId { get; set; }
        public long RoleId { get; set; }
        public int Status { get; set; } = 1;
    }

    public class UpdateUserInput : AddUserInput
    {
        public long Id { get; set; }
    }
}
