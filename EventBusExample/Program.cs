using Gt;
using Gt.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Loader;
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


            await eventBus.Run(new ExampleRequest() { Message = "事件命令发送" });///单独的命令

            //await eventBus.Publish(new CreateMessageNotification());/// 1对多 多播事件

            //var result = await eventBus.Send<CreateQQRequest, string>(new CreateQQRequest());///命令带返回值

            //result = await eventBus.Run<Shopping, string>(new Shopping());/// 命令带返回值  基于管道

            //Console.WriteLine(result);
            Console.ReadKey();
        }
    }
}
