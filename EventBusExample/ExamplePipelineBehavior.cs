using Gt;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventBusExample
{




    [Rule(1)]
    public class InMarketPipelineBehavior : IPipelineBehavior<Shopping, string>
    {

        public async Task<string> Handle(Shopping request, CancellationToken cancellationToken, Func<Shopping, Task<string>> next)
        {
            Console.WriteLine("进入商场");
            var status = await next(request);
            Console.WriteLine("购物结束");
            return "结束";
        }
    }

    [Rule(2)]
    public class DoShppingNikePipelineBehavior : IPipelineBehavior<Shopping, string>
    {
        public async Task<string> Handle(Shopping request, CancellationToken cancellationToken, Func<Shopping, Task<string>> next)
        {
            Console.WriteLine("边逛边买耐克");
            var status = await next(request);
            Console.WriteLine("购买耐克结束");
            return "耐克";
        }

        public async Task<string> Handle(Shopping request, CancellationToken cancellationToken, RequestHandlerDelegate<Shopping, string> next)
        {
            Console.WriteLine("边逛边买耐克");
            var status = await next();
            Console.WriteLine("购买耐克结束");
            return "耐克";
        }
    }

    [Rule(3)]
    public class DoShppingAdidasPipelineBehavior : IPipelineBehavior<Shopping, String>
    {
        public async Task<string> Handle(Shopping request, CancellationToken cancellationToken, Func<Shopping, Task<string>> next)
        {
            Console.WriteLine("边逛边买阿迪");
            var status = await next(request);
            Console.WriteLine("购买阿迪结束");
            return "阿迪";
        }
    }

}
