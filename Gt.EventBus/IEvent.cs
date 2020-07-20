using System;
using System.Collections.Generic;
using System.Text;

namespace Gt.EventBus
{

    public interface IEvent
    {

    }

    /// <summary>
    /// 1对1返回值
    /// </summary>
    /// <typeparam name="ITResponse"></typeparam>
    public interface IRequestResult<out ITResponse>
    {

    }

    /// <summary>
    /// 1对1无返回值
    /// </summary>
    public interface IRequest
    {

    }

    /// <summary>
    /// 通知1对多
    /// </summary>
    public interface INotification
    {

    }
}
