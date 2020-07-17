using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gt.EventBus
{
    public interface IEventBus
    {
        public Task<TOut> Publish<TEvent, TOut>(TEvent @event) where TEvent : IEvent;
    }
}
