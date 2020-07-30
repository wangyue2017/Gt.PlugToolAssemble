NetCore下基于内存的 命令查询的责任分离(cqrs)简易插件

使用简单 实现了 

IServiceCollection services = new ServiceCollection();

services.AddEventBus();

var provider = services.BuildServiceProvider();

var eventBus = provider.GetService<IEventBus>();
  
await eventBus.Send(new ExampleRequest() { Message = "事件命令发送" });///单独的命令

await eventBus.Publish(new CreateMessageNotification());/// 1对多 多播事件

var result = await eventBus.Send<CreateQQRequest, string>(new CreateQQRequest());///命令带返回值

result = await eventBus.Run<Shopping, string>(new Shopping());/// 命令带返回值  基于管道

