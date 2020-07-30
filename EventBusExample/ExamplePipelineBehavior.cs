using Gt;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventBusExample
{

    public class GoShoppingEventHandler : IRequestResultHandler<Shopping, string>
    {
        public Task<string> Handle(Shopping request, CancellationToken cancellationToken)
        {
            return Task.FromResult("购物结束了额");
        }
    }


    [Rule(1)]
    public class DoShppingAdidasiPipelineBehavior : IPipelineBehavior<Shopping, string>
    {
        public async Task<string> Handle(Shopping request, CancellationToken cancellationToken, Func<Task<string>> next)
        {
            var message = "我来到了阿迪展厅";
            var status = await next();
            Console.WriteLine(status);
            return message;
        }
    }

    [Rule(2)]
    public class DoShppingNikePipelineBehavior : IPipelineBehavior<Shopping, string>
    {
        public async Task<string> Handle(Shopping request, CancellationToken cancellationToken, Func<Task<string>> next)
        {
            var message = "我来到了耐克展厅";
            var status = await next();
            Console.WriteLine(status);
            return message;
        }
    }

    [Rule(3)]
    public class DoShppingEndPipelineBehavior : IPipelineBehavior<Shopping, String>
    {
        public async Task<string> Handle(Shopping request, CancellationToken cancellationToken, Func<Task<string>> next)
        {
            var message = "首次进入商场";
            request.Message = "首次进入商场";
            Console.WriteLine(message);
            var status = await next();
            Console.WriteLine(status);
            return message;
        }
    }

}
