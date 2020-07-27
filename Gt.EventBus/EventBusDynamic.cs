using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Gt
{
    public class EventBusDynamic
    {
        /// <summary>
        /// 代理类IRequestResultHandler
        /// </summary>
        public static ConcurrentDictionary<Type, Type> Proxy = new ConcurrentDictionary<Type, Type>();
    }
}
