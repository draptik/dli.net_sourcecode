using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.DI.AntiPatterns.DirectControl.Factory.StaticFactory.Complex
{
    public class AzureProductRepository : ProductRepository
    {
        public override IEnumerable<Product> GetFeaturedProducts()
        {
            throw new NotImplementedException();
        }
    }
}
