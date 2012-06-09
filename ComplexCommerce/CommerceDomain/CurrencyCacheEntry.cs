using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Commerce.Domain
{
    internal class CurrencyCacheEntry
    {
        private readonly decimal exchangeRate;
        private readonly DateTime expiration;

        internal CurrencyCacheEntry(decimal exchangeRate, DateTime expiration)
        {
            this.exchangeRate = exchangeRate;
            this.expiration = expiration;
        }

        internal decimal ExchangeRate
        {
            get { return this.exchangeRate; }
        }

        internal bool IsExpired
        {
            get { return TimeProvider.Current.UtcNow >= this.expiration; }
        }
    }
}
