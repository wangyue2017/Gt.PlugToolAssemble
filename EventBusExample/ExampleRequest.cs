using Gt;

namespace EventBusExample
{
    public class ExampleRequest : IRequest
    {
        public string Message { set; get; } = "发送个邮件试试";
    }

    public class CreateMessageNotification : INotification
    {
        public string Message { set; get; }
    }

    public class CreateQQRequest : IRequestResult<string>
    {

    }

    public class Result
    {
        public string Message { set; get; } = "我已经收到你的QQ消息了";
    }


    public class Shopping : IRequestResult<string>
    {
        public string Message { set; get; }
    }
}
