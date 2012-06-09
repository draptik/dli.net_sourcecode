using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Dependency.Lifetime
{
    // Don't expect this to make any sense. This class exists entirely to look good on paper and to
    // verify certain interactions.
    public abstract class BasketDiscountPolicy
    {
        public abstract IEnumerable<Product> GetDiscountedProducts();
    }
}
