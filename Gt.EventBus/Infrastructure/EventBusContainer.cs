using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Gt
{
    public class EventBusContainer
    {
        /// <summary>
        /// 注册规约
        /// </summary>
        public static readonly Dictionary<Type, RegisterStyle> Specifications = new Dictionary<Type, RegisterStyle>()
        {
            [typeof(IRequestHandler<>)] = RegisterStyle.One,
            [typeof(IRequestResultHandler<,>)] = RegisterStyle.One,
            [typeof(INotificationHandler<>)] = RegisterStyle.Many,
            [typeof(IPipelineBehavior<,>)] = RegisterStyle.Many,
        };

        public static Dictionary<Type, List<(Type Type, int Order)>> Provides = new Dictionary<Type, List<(Type Type, int Order)>>();
    }
}
