using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gt
{
    public delegate Task<TResponse> RequestHandlerDelegate<TRequestResult, TResponse>() where TRequestResult : IRequestResult<TResponse>;

    public interface IPipelineBehavior<TRequestResult, TResponse>  where TRequestResult : IRequestResult<TResponse>
    {
       
        Task<TResponse> Handle(TRequestResult request, CancellationToken cancellationToken, RequestHandlerDelegate<TRequestResult, TResponse> next);
    }
}
