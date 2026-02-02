using System.Collections.Generic;
using System.Linq;

namespace Strat.Infrastructure.Services.Abstractions
{
    public interface IPermissionService
    {
        void InitPermissions(IEnumerable<string> permissions);
        bool HasPermission(string code);
        void Clear();
    }
}
