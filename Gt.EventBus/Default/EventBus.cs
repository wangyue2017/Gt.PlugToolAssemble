using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gt
{
    public class EventBus : IEventBus
    {
        private readonly IServiceProvider _provider;
        public EventBus(IServiceProvider provider)
        => _provider = provider;

        public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification
        {
            using var scope = _provider.CreateScope();
            var eventHandlers = scope.ServiceProvider.GetService<IEnumerable<INotificationHandler<TNotification>>>();
            var list = new List<Task>();
            foreach (var eventHandler in eventHandlers)
                list.Add(eventHandler.Handle(notification, cancellationToken));
            return Task.WhenAll(list);
        }

        public Task<TResponse> SendToReturn<TRequestResult, TResponse>(TRequestResult request, CancellationToken cancellationToken = default) where TRequestResult : IRequestResult<TResponse>
        {
            if (EventBusDynamic.Proxy.TryGetValue(request.GetType(), out var implementationType))
            {
                using var scope = _provider.CreateScope();
                var eventHandler = scope.ServiceProvider.GetService(implementationType);
                var cc = eventHandler as IRequestResultHandler<TRequestResult, TResponse>;
                return Task.FromResult(default(TResponse));
                //return eventHandler?.Handle(request, cancellationToken);
            }
            return Task.FromResult(default(TResponse));
        }

        public Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest
        {
            using var scope = _provider.CreateScope();
            var eventHandler = scope.ServiceProvider.GetService<IRequestHandler<TRequest>>();
            return eventHandler?.Handle(request, cancellationToken);
        }

        public Task<TResponse> Send<TResponse>(IRequestResult<TResponse> request, CancellationToken cancellationToken = default)
        {
            Console.WriteLine(request.GetType().Name);
            return SendToReturn<IRequestResult<TResponse>, TResponse>(request, cancellationToken);
        }
    }
}
