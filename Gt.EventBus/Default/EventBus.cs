using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gt.EventBus.Default
{
    //public class EventBus : IEventBus
    //{
    //    private readonly IServiceProvider _provider;
    //    public EventBus(IServiceProvider provider)
    //    => _provider = provider;
    //    public Task<TOut> Publish<TEvent, TOut>(TEvent @event) where TEvent : IEvent
    //    {
    //        using var scope = _provider.CreateScope();
    //        var eventHandler = scope.ServiceProvider.GetService<IEventHandler<TEvent, TOut>>();
    //        return eventHandler?.HandleAsync(@event);
    //    }

    //}
}
