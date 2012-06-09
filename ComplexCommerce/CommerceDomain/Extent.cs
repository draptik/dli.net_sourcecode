using System;

namespace Ploeh.Samples.Commerce.Domain
{
    public class Extent
    {
        private readonly Product product;
        private Money total;

        public Extent(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException("item");
            }

            this.product = product;
        }

        public Product Product
        {
            get { return this.product; }
        }

        public int Quantity { get; set; }

        public Money Total
        {
            get { return this.total ?? this.Product.UnitPrice.Multiply(this.Quantity); }
            set { this.total = value; }
        }

        public DateTimeOffset Updated { get; set; }

        public Extent ConvertTo(Currency currency)
        {
            if (currency == null)
            {
                throw new ArgumentNullException("currency");
            }        

            var convertedExtent = this.WithItem(this.Product.ConvertTo(currency));
            convertedExtent.total = this.total == null ? null : this.total.ConvertTo(currency);
            return convertedExtent;
        }

        public void ResetTotal()
        {
            this.total = null;
        }

        public Extent WithItem(Product item)
        {
            var newExtent = new Extent(item);
            newExtent.Quantity = this.Quantity;
            newExtent.Updated = this.Updated;
            newExtent.total = this.total;
            return newExtent;
        }
    }
}
