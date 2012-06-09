using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Commerce.Domain
{
    public abstract class ProductRepository
    {
        public abstract IEnumerable<Product> GetFeaturedProducts();
    }
}
