using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.DI.AntiPatterns
{
    public class SqlProductRepository : ProductRepository
    {
        public SqlProductRepository(string connString)
        {
        }

        public override IEnumerable<Product> GetFeaturedProducts()
        {
            throw new NotImplementedException();
        }
    }
}
