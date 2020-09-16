using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
namespace Gt.Extensions
{


    public static class EventBusExtensions
    {

        public static void AddEventBus(this IServiceCollection services, Action<EventBusOptions> action = null)
        {

            var options = new EventBusOptions();
            action?.Invoke(options);
            foreach (var assembly in Directory.GetFiles(AppContext.BaseDirectory, "*.dll")
                                                           .Where(file => !file.Contains("System.", StringComparison.OrdinalIgnoreCase) && !file.Contains("Microsoft.", StringComparison.OrdinalIgnoreCase))
                                                           .Select(file => AssemblyLoadContext.Default.LoadFromAssemblyPath(file)))
            {
                try
                {
                    foreach (var type in assembly.GetTypes())
                    {
                        CanBeRegister(type);
                    }
                }
                catch (Exception ex)
                {

                   
                } 
            }
            services.AddEventBusCore(options);
        }


        private static void AddEventBusCore(this IServiceCollection services, EventBusOptions options)
        {
            services.AddSingleton<IEventBus, DefaultEventBus>();
            ///内存中存在需要注入的键值集合
            foreach (var keyValuePair in EventBusContainer.Provides)
            {

                if (keyValuePair.Key.Name == typeof(IRequestResultHandler<,>).Name || (keyValuePair.Key.Name == typeof(IPipelineBehavior<>).Name) || (keyValuePair.Key.Name == typeof(IPipelineBehavior<,>).Name))
                {
                    services.TryAddEnumerable(keyValuePair.Value.OrderByDescending(s => s.Order).Select(s => new ServiceDescriptor(keyValuePair.Key, s.Type, options.Lifetime)));
                }
                else
                {
                    var ImplementationType = keyValuePair.Value.FirstOrDefault();
                    services.Add(new ServiceDescriptor(keyValuePair.Key, ImplementationType.Type, options.Lifetime));
                }
                //if (keyValuePair.Value.Count > 1)
                //{
                //    services.TryAddEnumerable(keyValuePair.Value.OrderBy(s => s.Order).Select(s => new ServiceDescriptor(keyValuePair.Key, s.Type, options.Lifetime)));
                //}
                //else
                //{
                //    var ImplementationType = keyValuePair.Value.FirstOrDefault();
                //    if (keyValuePair.Key.Name == typeof(IRequestResultHandler<,>).Name)
                //    {
                //        foreach (var argument in keyValuePair.Key.GenericTypeArguments)
                //        {
                //            if (argument.GetTypeInfo().ImplementedInterfaces.Any(c => c.Name == typeof(IRequestResult<>).Name))
                //                EventBusDynamic.Proxy[argument] = keyValuePair.Key;
                //        }
                //    }
                //    services.Add(new ServiceDescriptor(keyValuePair.Key, ImplementationType.Type, options.Lifetime));
                //}

            }
        }


        private static void CanBeRegister(Type type)
        {
            if (!type.IsEnum && !type.IsAbstract && !type.IsInterface && type.GetInterfaces().Any())
            {
                foreach (var @interface in type.GetInterfaces())
                {
                    ///符合规则限制的注入接口类型
                    if (EventBusContainer.Specifications.Any(o => o.Key.Name == @interface.Name))
                    {
                        var registerStyle = EventBusContainer.Specifications.FirstOrDefault(o => o.Key.Name == @interface.Name).Value;

                        if (!EventBusContainer.Provides.TryGetValue(@interface, out var list))
                            list = new List<(Type Type, int Order)>() { (type, type.GetCustomAttribute<OrderRuleAttribute>()?.Order ?? 0) };
                        else
                        {
                            if (registerStyle == RegisterStyle.Many)
                                list.Add((type, type.GetCustomAttribute<OrderRuleAttribute>()?.Order ?? 0));
                            else
                                list = new List<(Type Type, int Order)>() { (type, type.GetCustomAttribute<OrderRuleAttribute>()?.Order ?? 0) };
                        }
                        EventBusContainer.Provides[@interface] = list;
                    }
                }
            }
        }

    }
}
