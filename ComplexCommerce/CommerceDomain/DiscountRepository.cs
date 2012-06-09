using System.Collections.Generic;

namespace Ploeh.Samples.Commerce.Domain
{
    public abstract class DiscountRepository
    {
        public abstract IEnumerable<Product> GetDiscountedProducts();
    }
}
