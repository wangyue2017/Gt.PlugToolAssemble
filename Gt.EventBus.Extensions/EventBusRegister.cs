using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections.Concurrent;

namespace Gt.Extensions
{
    internal class EventBusRegister
    {
        /// <summary>
        /// 注册规约
        /// </summary>
        internal static readonly Dictionary<Type, RegisterStyle> _specifications = new Dictionary<Type, RegisterStyle>()
        {
            [typeof(IRequestHandler<>)]= RegisterStyle.One,
            [typeof(IRequestResultHandler<,>)] = RegisterStyle.One,
            [typeof(INotificationHandler<>)] = RegisterStyle.Many,
        };

        internal static Dictionary<Type, List<Type>> _containers = new Dictionary<Type, List<Type>>();
    }
}
