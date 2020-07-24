using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

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
            services.AddRegister();
            if (options.Types.Count < 1 && options.Assembly == null)
            {
                var assemblys = Directory.GetFiles(AppContext.BaseDirectory, "*.dll")
                                                            .Where(file => !file.Contains("System.", StringComparison.OrdinalIgnoreCase) && !file.Contains("Microsoft.", StringComparison.OrdinalIgnoreCase))
                                                            .Select(file => Assembly.LoadFile(file))
                                                            .ToList();

                EventBusHandleInjection(services, assemblys, options.Lifetime);
            }

        }


        private static void AddRegister(this IServiceCollection services)
        {
            services.AddSingleton<IEventBus, EventBus>();
        }


        private static void EventBusHandleInjection(this IServiceCollection services, List<Assembly> assemblys, ServiceLifetime lifetime)
        {
            assemblys?.SelectMany(s => s.DefinedTypes)
                               .ToList()
                               .ForEach(s =>
                                {
                                    CanBeRegister(s);
                                });
            if (EventBusRegister.Containers.Count > 0)
            {
                foreach (var Container in EventBusRegister.Containers)
                {
                    if (Container.Value.Count > 1)
                        services.TryAddEnumerable(Container.Value.ConvertAll(s => new ServiceDescriptor(Container.Key, s, lifetime)));
                    else
                    {
                        var @interface = Container.Key;
                        var @class = Container.Value.FirstOrDefault();
                        services.AddScoped(@interface, @class);
                    }
                }
            }
        }

        private static void CanBeRegister(Type type)
        {
            EventBusRegister.Interfaces.ForEach(o =>
            {
                ///首先过滤当前类非抽象类或接口 并且包含接口
                if (!type.IsAbstract && !type.IsInterface && type.GetInterfaces().Any())
                {
                    foreach (var @interface in type.GetInterfaces())
                    {
                        ///内置的接口符合
                        if (@interface.Name == o.Type.Name)
                        {
                            if (EventBusRegister.Containers.Any(c => c.Key.Name == o.Type.Name))
                            {
                                if (o.Style == RegisterStyle.One)
                                    EventBusRegister.Containers[@interface] = new List<Type>() { type };
                                else
                                {
                                    var list = EventBusRegister.Containers[@interface];
                                    if (!list.Any(o => o.Name == type.Name))
                                    {
                                        list.Add(type);
                                        EventBusRegister.Containers[@interface] = list;
                                    }
                                }
                            }
                            else
                                EventBusRegister.Containers[@interface] = new List<Type>() { type };
                        }
                    }
                }
            });
        }







    }
}
