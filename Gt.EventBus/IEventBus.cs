using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gt
{
    public interface IEventBus
    {

       Task<TResponse> Send<TResponse>(IRequestResult<TResponse> request, CancellationToken cancellationToken = default);

        Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest;

        Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification;

    }
}
