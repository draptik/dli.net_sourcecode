using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.Samples.Commerce.Domain;

namespace Ploeh.Samples.CommerceService
{
    public class ContractMapper : IContractMapper
    {
        #region IContractMapper Members

        public MoneyContract Map(Money money)
        {
            if (money == null)
            {
                throw new ArgumentNullException("money");
            }

            var mc = new MoneyContract();
            mc.Amount = money.Amount;
            mc.CurrencyCode = money.CurrencyCode;
            return mc;
        }

        public Money Map(MoneyContract moneyContract)
        {
            if (moneyContract == null)
            {
                throw new ArgumentNullException("moneyContract");
            }

            return new Money(moneyContract.Amount, moneyContract.CurrencyCode);
        }

        public ProductContract Map(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException("product");
            }

            var pc = new ProductContract();
            pc.Id = product.Id;
            pc.Name = product.Name;
            pc.UnitPrice = this.Map(product.UnitPrice);
            return pc;
        }

        public IEnumerable<ProductContract> Map(IEnumerable<Product> products)
        {
            if (products == null)
            {
                throw new ArgumentNullException("products");
            }        

            foreach (var p in products)
            {
                yield return this.Map(p);
            }
        }

        public Product Map(ProductContract productContract)
        {
            if (productContract == null)
            {
                throw new ArgumentNullException("productContract");
            }

            return new Product(productContract.Id, productContract.Name, this.Map(productContract.UnitPrice));
        }

        #endregion
    }
}
