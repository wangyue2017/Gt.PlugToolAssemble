using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gt
{

    public interface IPipelineBehavior<TRequestResult, TResponse> where TRequestResult : IRequestResult<TResponse>
    {

        Task<TResponse> Handle(TRequestResult request, CancellationToken cancellationToken, Func<Task<TResponse>> next);
    }


    public interface IPipelineBehavior<TRequest> where TRequest : IRequest
    {
        Task Handle(TRequest request, CancellationToken cancellationToken, Func<Task> next);
    }
}
