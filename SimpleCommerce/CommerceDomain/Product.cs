using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;

namespace Ploeh.Samples.Commerce.Domain
{
    public class Product
    {
        public string Name { get; set; }

        public decimal UnitPrice { get; set; }

        public DiscountedProduct ApplyDiscountFor(IPrincipal user)
        {
            var discount = user.IsInRole("PreferredCustomer") ? .95m : 1;
            return new DiscountedProduct(this.Name, this.UnitPrice * discount);
        }
    }
}
