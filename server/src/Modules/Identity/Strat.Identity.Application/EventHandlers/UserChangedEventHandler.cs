using Strat.Identity.Domain.Events;
using Strat.Shared.Abstractions;
using Strat.Shared.Constants;
using System.Threading.Tasks;

namespace Strat.Identity.Application.EventHandlers;

/// <summary>
/// 用户变更事件处理：负责清理相关缓存
/// </summary>
public class UserChangedEventHandler(ICache cache) : IEventHandler<UserChangedEvent>
{
    private readonly ICache _cache = cache;
    private const string PermissionCacheKey = "Auth:Permissions:{0}";
    private const string RouterCacheKey = "Auth:Routers:{0}";

    public Task HandleAsync(UserChangedEvent @event)
    {
        var permissionKey = string.Format(PermissionCacheKey, @event.UserId);
        var routerKey = string.Format(RouterCacheKey, @event.UserId);

        _cache.Remove(permissionKey);
        _cache.Remove(routerKey);

        return Task.CompletedTask;
    }
}
