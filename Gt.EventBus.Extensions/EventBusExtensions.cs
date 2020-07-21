using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Gt.EventBus.Extensions
{

    public enum RegisterStyle
    {
        One = 0,
        Many = 1
    }


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

            //if (options.Assemblys.Count > 0)
            //    services.EventBusHandleInjection(options.Assemblys, options.Lifetime);
        }


        private static void EventBusHandleInjection(this IServiceCollection services, List<Assembly> assemblys, ServiceLifetime lifetime)
        {

            assemblys?.SelectMany(s => s.DefinedTypes)
                               .ToList()
                               .ForEach(s =>
                                {

                                });

            //foreach (var item in assemblys)
            //{
            //    var assembly = System.Reflection.Assembly.Load(item);
            //    assembly?.


            //    foreach (var type in assembly.ExportedTypes)
            //    {
            //        if (!type.IsAbstract && !type.IsInterface && !type.IsGenericType && type.CustomAttributes != null)
            //        {
            //            var interfaces = type.GetInterfaces();
            //            foreach (var face in interfaces)
            //            {
            //                //if (face is typeof(IEventHandler<,>))
            //                //    services.Add(new ServiceDescriptor(face, type, lifetime));
            //            }
            //        }
            //    }
            //}

        }

        private static void CanBeRegister(Type type)
        {
            EventBusRegister.Interfaces.ForEach(o => 
            {
                ///首先过滤当前类非抽象类或接口 并且包含接口
                if (!type.IsAbstract && !type.IsInterface&&type.GetInterfaces().Any())
                { 
                       
                }
            });
        }







    }
}
