using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Commerce.Domain
{
    public abstract class BasketDiscountPolicy
    {
        public abstract Basket Apply(Basket basket);
    }
}
