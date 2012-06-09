using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;

namespace Ploeh.Samples.Commerce.Domain
{
    public class DefaultCustomerDiscountPolicy
    {
        public Product Apply(Product product, IPrincipal customer)
        {
            var discount = customer.IsInRole("PreferredCustomer") ? .95m : 1;
            return product.WithUnitPrice(product.UnitPrice.Multiply(discount));
        }
    }
}
