using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gt.EventBus.Default
{
    public class EventBus : IEventBus
    {
        private readonly IServiceProvider _provider;
        public EventBus(IServiceProvider provider)
        => _provider = provider;

        public Task Publish(INotification notification, CancellationToken cancellationToken = default)
        {
            using var scope = _provider.CreateScope();
            var eventHandler = scope.ServiceProvider.GetService<IINotificationHandler<INotification>>();
            return eventHandler?.Handle(notification, cancellationToken);
        }

        public Task<TResponse> Send<TResponse>(IRequestResult<TResponse> request, CancellationToken cancellationToken = default)
        {
            using var scope = _provider.CreateScope();
            var eventHandler = scope.ServiceProvider.GetService<IRequestResultHandler<IRequestResult<TResponse>,TResponse>> ();
            return eventHandler?.Handle(request, cancellationToken);
        }

        public Task Send(IRequest request, CancellationToken cancellationToken = default)
        {
            using var scope = _provider.CreateScope();
            var eventHandler = scope.ServiceProvider.GetService<IRequestHandler<IRequest>>();
            return eventHandler?.Handle(request, cancellationToken);
        }
    }
}
