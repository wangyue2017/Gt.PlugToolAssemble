using System;
using System.Collections.Generic;
using System.Text;

namespace Gt
{
    public class OrderRuleAttribute :  Attribute, IOrderRule
    {
        public int _order = 0;
        public OrderRuleAttribute(int order = 0)
        {
            _order = order;
        }

        public int Order => _order;
    }
}
