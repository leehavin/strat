using Strat.Shared.Abstractions;

namespace Strat.Identity.Domain.Events;

/// <summary>
/// 用户变更事件 (用于跨模块协作或异步清理缓存)
/// </summary>
public class UserChangedEvent : IEvent
{
    public long UserId { get; }
    public DateTime OccurredOn { get; } = DateTime.Now;

    public UserChangedEvent(long userId)
    {
        UserId = userId;
    }
}
