using Gt;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventBusExample
{
    public class CreateEmailEventHandler : IRequestHandler<CreateEmailRequest>
    {
        public Task Handle(CreateEmailRequest request, CancellationToken cancellationToken)
        {
            Console.WriteLine(request.Message);
            return Task.CompletedTask;
        }
    }
}
