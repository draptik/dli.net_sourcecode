using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Commerce.Domain
{
    public class CampaignItem
    {
        private readonly Product product;
        private readonly bool isFeatured;
        private readonly Money discountPrice;

        public CampaignItem(Product product, bool isFeatured, Money discountPrice)
        {
            if (product == null)
            {
                throw new ArgumentNullException("product");
            }

            this.product = product;
            this.isFeatured = isFeatured;
            this.discountPrice = discountPrice;
        }

        public Money DiscountPrice
        {
            get { return this.discountPrice; }
        }

        public bool IsFeatured
        {
            get { return this.isFeatured; }
        }

        public Product Product
        {
            get { return this.product; }
        }
    }
}
