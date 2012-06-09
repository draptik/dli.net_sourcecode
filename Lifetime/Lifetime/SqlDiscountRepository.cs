using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Dependency.Lifetime
{
    // This implementation is a total fake and is only used to demonstrate how to configure
    // lifetimes of dependencies.
    public partial class SqlDiscountRepository : DiscountRepository
    {
        private readonly List<Product> products;

        public SqlDiscountRepository(string connString)
        {
            this.products = new List<Product>();
        }

        public override IList<Product> Products
        {
            get { return this.products; }
        }

        public override IEnumerable<Product> GetDiscountedProducts()
        {
            return this.products;
        }
    }

    public partial class SqlDiscountRepository : IDisposable
    {
        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }

}
