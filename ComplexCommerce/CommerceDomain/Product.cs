using System;
using System.Security.Principal;

namespace Ploeh.Samples.Commerce.Domain
{
    public class Product
    {
        private readonly int id;
        private readonly string name;
        private readonly Money unitPrice;

        public Product(int id, string name, Money unitPrice)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            this.id = id;
            this.name = name;
            this.unitPrice = unitPrice;
        }

        public int Id
        {
            get { return this.id; }
        }

        public string Name
        {
            get { return this.name; }
        }

        public Money UnitPrice
        {
            get { return this.unitPrice; }
        }

        public Product ApplyDiscountFor(IPrincipal user)
        {
            var dicountPolicy = new DefaultCustomerDiscountPolicy();
            return dicountPolicy.Apply(this, user);
        }

        public Product ConvertTo(Currency currency)
        {
            if (currency == null)
            {
                throw new ArgumentNullException("currency");
            }       

            return this.WithUnitPrice(this.UnitPrice.ConvertTo(currency));
        }

        public Product WithUnitPrice(Money unitPrice)
        {
            return new Product(this.Id, this.Name, unitPrice);
        }
    }
}
