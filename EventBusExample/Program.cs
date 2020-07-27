using Gt;
using Gt.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace EventBusExample
{
    class Program
    {
        async static Task Main(string[] args)
        {
            IServiceCollection services = new ServiceCollection();
            services.AddEventBus();
            var provider = services.BuildServiceProvider();
            var eventBus = provider.GetService<IEventBus>();
           // var eventBus = provider.GetService<IRequestResultHandler<CreateQQRequest,Result>> ();
            // await eventBus.Send(new ExampleRequest());
            //await eventBus.Publish(new CreateMessageNotification());
            var result = await eventBus.Send(new CreateQQRequest());
            Console.ReadKey();
        }
    }
}
                      