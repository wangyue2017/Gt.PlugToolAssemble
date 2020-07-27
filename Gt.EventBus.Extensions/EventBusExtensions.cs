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

    public enum RegisterStyle
    {
        One = 0,
        Many = 1
    }


    public static class EventBusExtensions
    {

        public static void AddEventBus(this IServiceCollection services, Action<EventBusOptions> action = null)
        {

            var options = new EventBusOptions();
            action?.Invoke(options);
            if (options.Types.Count < 1 && options.Assembly == null)
            {
                foreach (var assembly in Directory.GetFiles(AppContext.BaseDirectory, "*.dll")
                                                            .Where(file => !file.Contains("System.", StringComparison.OrdinalIgnoreCase) && !file.Contains("Microsoft.", StringComparison.OrdinalIgnoreCase))
                                                            .Select(file => AssemblyLoadContext.Default.LoadFromAssemblyPath(file))
                                                            .ToList())
                {
                    foreach (var type in assembly.GetTypes())
                    {
                        CanBeRegister(type);
                    }
                }

            }
            services.AddEventBusCore(options);
        }


        private static void AddEventBusCore(this IServiceCollection services, EventBusOptions options)
        {
            services.AddSingleton<IEventBus, EventBus>();
            ///内存中存在需要注入的键值集合
            foreach (var keyValuePair in EventBusRegister._containers)
            {
                if (keyValuePair.Value.Count > 1)
                {
                    //if (keyValuePair.Key.Name == typeof(INotificationHandler<>).Name)
                        services.TryAddEnumerable(keyValuePair.Value.ConvertAll(s => new ServiceDescriptor(keyValuePair.Key, s, options.Lifetime)));
                }
                else
                    services.Add(new ServiceDescriptor(keyValuePair.Key, keyValuePair.Value.FirstOrDefault(), options.Lifetime));
            }
        }


        private static void CanBeRegister(Type type)
        {
            if (!type.IsEnum && !type.IsAbstract && !type.IsInterface && type.GetInterfaces().Any())
            {
                foreach (var @interface in type.GetInterfaces())
                {
                    ///符合规则限制的注入接口类型
                    if (EventBusRegister._specifications.Any(o => o.Key.Name == @interface.Name))
                    {
                        var registerStyle = EventBusRegister._specifications.FirstOrDefault(o => o.Key.Name == @interface.Name).Value;

                        if (!EventBusRegister._containers.TryGetValue(@interface, out var list))
                            list = new List<Type>() { type };
                        else
                        {
                            if (registerStyle == RegisterStyle.Many)
                                list.Add(type);
                            else
                                list = new List<Type>() { type };
                        }
                        EventBusRegister._containers[@interface] = list;
                    }
                }
            }
        }
    }
}
