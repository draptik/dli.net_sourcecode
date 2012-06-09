using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.DI.AntiPatterns
{
    public abstract class ProductRepository
    {
        public abstract IEnumerable<Product> GetFeaturedProducts();
    }
}
