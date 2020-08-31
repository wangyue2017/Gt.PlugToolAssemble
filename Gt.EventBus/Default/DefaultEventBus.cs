using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gt
{
    public class DefaultEventBus : IEventBus
    {
        private readonly IServiceProvider _provider;
        public DefaultEventBus(IServiceProvider provider)
        => _provider = provider;

        public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification
        {
            using var scope = _provider.CreateScope();
            var eventHandlers = scope.ServiceProvider.GetService<IEnumerable<INotificationHandler<TNotification>>>();
            var list = new List<Task>();
            foreach (var eventHandler in eventHandlers)
                list.Add(eventHandler.Handle(notification, cancellationToken));
            await Task.WhenAll(list);
        }

        public async Task<TResponse> Run<TRequestResult, TResponse>(TRequestResult request, CancellationToken cancellationToken = default) where TRequestResult : IRequestResult<TResponse>
        {
            using var scope = _provider.CreateScope();

            var eventHandler = scope.ServiceProvider.GetService<IRequestResultHandler<TRequestResult, TResponse>>();

            var pipelines = scope.ServiceProvider.GetService<IEnumerable<IPipelineBehavior<TRequestResult, TResponse>>>();
            if (pipelines != null && pipelines.Count() > 0)
            {
                var endRequest = new Func<Task<TResponse>>(() => eventHandler.Handle(request, cancellationToken));

                return await pipelines.Aggregate(endRequest, (next, pipeline) => () => pipeline.Handle(request, cancellationToken, next))();
            }
            else return await eventHandler.Handle(request, cancellationToken);
        }

        public async Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest
        {
            using var scope = _provider.CreateScope();
            var eventHandler = scope.ServiceProvider.GetService<IRequestHandler<TRequest>>();
            await eventHandler?.Handle(request, cancellationToken);
        }



        public async Task Run<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest
        {
            using var scope = _provider.CreateScope();
            var eventHandler = scope.ServiceProvider.GetService<IRequestHandler<TRequest>>();
            var pipelines = scope.ServiceProvider.GetService<IEnumerable<IPipelineBehavior<TRequest>>>();
            if (pipelines != null && pipelines.Count() > 0)
            {
                var endRequest = new Func<Task>(() => eventHandler.Handle(request, cancellationToken));
                await pipelines.Aggregate(endRequest, (next, pipeline) => async () => await pipeline.Handle(request, cancellationToken, next))();
            }
            else
                await eventHandler?.Handle(request, cancellationToken);
        }


        public async Task<TResponse> Send<TRequestResult, TResponse>(TRequestResult request, CancellationToken cancellationToken = default) where TRequestResult : IRequestResult<TResponse>
        {
            using var scope = _provider.CreateScope();
            var eventHandler = scope.ServiceProvider.GetService<IRequestResultHandler<TRequestResult, TResponse>>();
            return await eventHandler?.Handle(request, cancellationToken);
        }

        private Task<TResponse> SendToReturn<TRequestResult, TResponse>(TRequestResult request, CancellationToken cancellationToken = default) where TRequestResult : IRequestResult<TResponse>
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


    }
}
