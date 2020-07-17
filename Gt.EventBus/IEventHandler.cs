using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gt.EventBus
{
    public interface IEventHandler<TEvent, TOut> where TEvent : IEvent
    {
        Task<TOut> HandleAsync(TEvent @event);
    }
}
