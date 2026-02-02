using Strat.Infrastructure.Services.Abstractions;
using System.Collections.Generic;
using System.Linq;

namespace Strat.Infrastructure.Services.Implements
{
    public class PermissionService : IPermissionService
    {
        private readonly HashSet<string> _permissions = new HashSet<string>();

        public void InitPermissions(IEnumerable<string> permissions)
        {
            _permissions.Clear();
            if (permissions != null)
            {
                foreach (var p in permissions)
                {
                    _permissions.Add(p);
                }
            }
        }

        public bool HasPermission(string code)
        {
            if (string.IsNullOrEmpty(code)) return true;
            
            // 超级管理员权限
            if (_permissions.Contains("*:*:*") || _permissions.Contains("admin"))
                return true;

            return _permissions.Contains(code);
        }

        public void Clear()
        {
            _permissions.Clear();
        }
    }
}
