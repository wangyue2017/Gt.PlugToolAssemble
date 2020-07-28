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


            await eventBus.Send(new ExampleRequest() { Message = "事件命令发送" });///单独的命令

            await eventBus.Publish(new CreateMessageNotification());/// 1对多 多播事件

            var result = await eventBus.Send<CreateQQRequest, string>(new CreateQQRequest());/// 命令带返回值

            Console.WriteLine(result);
            Console.ReadKey();
        }
    }
}
