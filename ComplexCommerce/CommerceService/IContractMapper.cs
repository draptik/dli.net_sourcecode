using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.Samples.Commerce.Domain;

namespace Ploeh.Samples.CommerceService
{
    public interface IContractMapper
    {
        MoneyContract Map(Money money);

        Money Map(MoneyContract moneyContract);

        ProductContract Map(Product product);

        IEnumerable<ProductContract> Map(IEnumerable<Product> products);

        Product Map(ProductContract productContract);
    }
}
