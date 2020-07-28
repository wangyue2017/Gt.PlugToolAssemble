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
            foreach (var keyValuePair in EventBusContainer.Provides)
            {
                if (keyValuePair.Value.Count > 1)
                    services.TryAddEnumerable(keyValuePair.Value.ConvertAll(s => new ServiceDescriptor(keyValuePair.Key, s, options.Lifetime)));
                else
                {
                    var ImplementationType = keyValuePair.Value.FirstOrDefault();
                    if (keyValuePair.Key.Name == typeof(IRequestResultHandler<,>).Name)
                    {
                        foreach (var argument in keyValuePair.Key.GenericTypeArguments)
                        {
                            if (argument.GetTypeInfo().ImplementedInterfaces.Any(c => c.Name == typeof(IRequestResult<>).Name))
                                EventBusDynamic.Proxy[argument] = keyValuePair.Key;
                        }
                    }
                    services.Add(new ServiceDescriptor(keyValuePair.Key, ImplementationType, options.Lifetime));
                }

            }
        }


        private static void CanBeRegister(Type type)
        {
            if (type.IsPublic && !type.IsEnum && !type.IsAbstract && !type.IsInterface && type.GetInterfaces().Any())
            {
                foreach (var @interface in type.GetInterfaces())
                {
                    ///符合规则限制的注入接口类型
                    if (EventBusContainer.Specifications.Any(o => o.Key.Name == @interface.Name))
                    {
                        var registerStyle = EventBusContainer.Specifications.FirstOrDefault(o => o.Key.Name == @interface.Name).Value;

                        if (!EventBusContainer.Provides.TryGetValue(@interface, out var list))
                            list = new List<Type>() { type };
                        else
                        {
                            if (registerStyle == RegisterStyle.Many)
                                list.Add(type);
                            else
                                list = new List<Type>() { type };
                        }
                        EventBusContainer.Provides[@interface] = list;
                    }
                }
            }
        }
    }
}
