using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;

namespace Ploeh.Samples.DI.AntiPatterns
{
    public class Product
    {
        public Product ApplyDiscountFor(IPrincipal user)
        {
            return this;
        }
    }
}
