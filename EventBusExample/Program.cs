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
            await eventBus.Send(new CreateEmailRequest());
            Console.ReadKey();
        }
    }
}
