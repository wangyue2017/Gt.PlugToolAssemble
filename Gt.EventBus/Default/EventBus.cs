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

        public Task Publish(INotification notification, CancellationToken cancellationToken = default)
        {
            using var scope = _provider.CreateScope();
            var eventHandlers = scope.ServiceProvider.GetService<IEnumerable<IINotificationHandler<INotification>>>();
            var list = new List<Task>();
            foreach (var eventHandler in eventHandlers)
                list.Add(eventHandler.Handle(notification, cancellationToken));
            return Task.WhenAll(list);
        }

        public Task<TResponse> Send<TResponse>(IRequestResult<TResponse> request, CancellationToken cancellationToken = default)
        {
            using var scope = _provider.CreateScope();
            var eventHandler = scope.ServiceProvider.GetService<IRequestResultHandler<IRequestResult<TResponse>, TResponse>>();
            return eventHandler?.Handle(request, cancellationToken);
        }

        public Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest
        {
            using var scope = _provider.CreateScope();
            var eventHandler = scope.ServiceProvider.GetService<IRequestHandler<TRequest>>();
            return eventHandler?.Handle(request, cancellationToken);
        }
    }
}
