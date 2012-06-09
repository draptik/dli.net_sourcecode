using System.Collections.Generic;
using Ploeh.Samples.Mary.ECommerce.Data.Sql;

namespace Ploeh.Samples.Mary.ECommerce.Domain
{
    interface IProductService
    {
        IEnumerable<Product> GetFeaturedProducts(
            bool isCustomerPreferred);
    }
}
