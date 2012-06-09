using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Commerce.Domain
{
    public abstract partial class Currency
    {
        public abstract string Code { get; }

        public abstract decimal GetExchangeRateFor(
            string currencyCode);
    }

    public abstract partial class Currency
    {
        public abstract void UpdateExchangeRate(
            string currencyCode, decimal rate);
    }
}
