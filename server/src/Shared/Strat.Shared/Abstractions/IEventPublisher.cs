namespace Strat.Shared.Abstractions;

/// <summary>
/// 领域事件接口
/// </summary>
public interface IEvent
{
    DateTime OccurredOn { get; }
}

/// <summary>
/// 事件处理程序接口
/// </summary>
/// <typeparam name="TEvent"></typeparam>
public interface IEventHandler<in TEvent> where TEvent : IEvent
{
    Task HandleAsync(TEvent @event);
}

/// <summary>
/// 事件发布器接口
/// </summary>
public interface IEventPublisher
{
    Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent;
}
