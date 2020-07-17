using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Gt.EventBus.Extensions
{
    public static class EventBusExtensions
    {
        public static void AddEventBus(this IServiceCollection services, Action<EventBusOptions> action)
        {
            if (action == null || action == default(Action<EventBusOptions>))
                throw new ArgumentNullException("注入的程序集加载项不能为空");
            var options = new EventBusOptions();
            action.Invoke(options);

            if (options.Assemblys.Count < 1 && options.Types.Count < 1 && options.Assembly == null)
                throw new ArgumentNullException("注入的程序集加载项不能为空");

            if (options.Assemblys.Count > 0)
                services.EventBusHandleInjection(options.Assemblys, options.Lifetime);
        }


        private static void EventBusHandleInjection(this IServiceCollection services, List<string> assemblys, ServiceLifetime lifetime)
        {
            foreach (var ietm in assemblys)
            {
                var assembly = System.Reflection.Assembly.Load(ietm);
                foreach (var type in assembly.ExportedTypes)
                {
                    if (!type.IsAbstract && !type.IsInterface && !type.IsGenericType && type.CustomAttributes != null)
                    {
                        var interfaces = type.GetInterfaces();
                        foreach (var face in interfaces)
                        {
                            services.Add(new ServiceDescriptor(face, type, lifetime));
                        }
                    }
                }
            }

        }









    }
}
