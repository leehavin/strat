using Microsoft.Extensions.DependencyInjection;
using Strat.Shared.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Strat.Infrastructure.Services
{
    public class EventPublisher(IServiceProvider serviceProvider) : IEventPublisher
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent
        {
            if (@event == null) return;

            // 获取所有处理该事件的处理程序
            var handlers = _serviceProvider.GetServices<IEventHandler<TEvent>>();
            
            foreach (var handler in handlers)
            {
                await handler.HandleAsync(@event);
            }
        }
    }
}
