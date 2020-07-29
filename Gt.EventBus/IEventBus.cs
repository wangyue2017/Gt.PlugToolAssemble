using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gt
{
    public interface IEventBus
    {

        Task<TResponse> Send<TRequestResult, TResponse>(TRequestResult request, CancellationToken cancellationToken = default) where TRequestResult : IRequestResult<TResponse>;

        Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest;

        Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification;

        /// <summary>
        /// 特殊管道
        /// </summary>
        /// <typeparam name="TRequestResult"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TResponse> Run<TRequestResult, TResponse>(TRequestResult request, CancellationToken cancellationToken = default) where TRequestResult : IRequestResult<TResponse>;

    }
}
