using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Dependency.Lifetime
{
    public abstract class DiscountRepository
    {
        public abstract IList<Product> Products { get; }

        public abstract IEnumerable<Product> GetDiscountedProducts();
    }
}
