using Gt;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventBusExample
{
    public class ExampleEventHandler : IRequestHandler<ExampleRequest>
    {
        public Task Handle(ExampleRequest request, CancellationToken cancellationToken)
        {
            Console.WriteLine(request.Message);
            return Task.CompletedTask;
        }
    }


    public class CreateMessageNotificationEventHandler : INotificationHandler<CreateMessageNotification>
    {
        public int Order { get; set; } = 0;

        public Task Handle(CreateMessageNotification notification, CancellationToken cancellationToken)
        {
            Console.WriteLine("001-收到广播消息");
            return Task.CompletedTask;
        }
    }

    public class CreateWeiXinNotificationEventHandler : INotificationHandler<CreateMessageNotification>
    {
        public int Order { get; set; } = 1;

        public Task Handle(CreateMessageNotification notification, CancellationToken cancellationToken)
        {
            Console.WriteLine("002-收到广播消息");
            return Task.CompletedTask;
        }
    }



    public class CreateQQEventHandler : IRequestResultHandler<CreateQQRequest, string>
    {
        public Task<string> Handle(CreateQQRequest request, CancellationToken cancellationToken)
        => Task.FromResult("这是一条QQ命令");
    }
}
