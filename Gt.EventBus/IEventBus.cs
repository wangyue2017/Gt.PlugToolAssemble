using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gt.EventBus
{
    public interface IEventBus
    {

        Task<TResponse> Send<TResponse>(IRequestResult<TResponse> request, CancellationToken cancellationToken = default);

        Task Send(IRequest request, CancellationToken cancellationToken = default);

        Task Publish(INotification notification, CancellationToken cancellationToken = default);

    }
}
