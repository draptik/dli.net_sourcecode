using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Commerce.Domain
{
    public class DiscountedProduct
    {
        private readonly string name;
        private readonly decimal unitPrice;

        public DiscountedProduct(string name, decimal unitPrice)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            this.name = name;
            this.unitPrice = unitPrice;
        }

        public string Name
        {
            get { return this.name; }
        }

        public decimal UnitPrice
        {
            get { return this.unitPrice; }
        }
    }
}
