using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Commerce.Domain
{
    public abstract class CurrencyProvider
    {
        public abstract Currency GetCurrency(string currencyCode);
    }
}
