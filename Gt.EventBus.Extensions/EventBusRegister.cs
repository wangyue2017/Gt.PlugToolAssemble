using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections.Concurrent;

namespace Gt.Extensions
{
  public  class EventBusRegister
    {
        public static readonly List<(Type Type, RegisterStyle Style)> Interfaces = new List<(Type Type, RegisterStyle Style)>()
        {
            (typeof(IRequestHandler<>), RegisterStyle.One),
            (typeof(IRequestResultHandler<,>),RegisterStyle.One),
            (typeof(IINotificationHandler<>), RegisterStyle.Many),
        };

        public static ConcurrentDictionary<Type, List<Type>> Containers = new ConcurrentDictionary<Type, List<Type>>();
    }
}
