using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gt
{


    public interface IRequestResultHandler<in TRequestResult, TResponse>
        where TRequestResult : IRequestResult<TResponse> 
    {
        Task<TResponse> Handle(TRequestResult request, CancellationToken cancellationToken);
    }


    public interface IRequestHandler<TRequest>
        where TRequest : IRequest
    {
        Task Handle(TRequest request, CancellationToken cancellationToken);
    }

    public interface INotificationHandler<TNotification>
    where TNotification : INotification
    {

        public int Order { set; get; } 
        Task Handle(TNotification notification, CancellationToken cancellationToken);
    }

}
