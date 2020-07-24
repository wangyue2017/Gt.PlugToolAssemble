using Gt;

namespace EventBusExample
{
    public class CreateEmailRequest : IRequest
    {
        public string Message { set; get; } = "发送个邮件试试";
    }
}
