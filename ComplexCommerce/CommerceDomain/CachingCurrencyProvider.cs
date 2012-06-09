using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Commerce.Domain
{
    public class CachingCurrencyProvider : CurrencyProvider
    {
        private readonly CurrencyProvider innerProvider;
        private readonly TimeSpan cacheTimeout;
        private readonly Dictionary<string, CachingCurrency> cache;

        public CachingCurrencyProvider(CurrencyProvider innerProvider, TimeSpan cacheTimeout)
        {
            if (innerProvider == null)
            {
                throw new ArgumentNullException("innerProvider");
            }

            this.innerProvider = innerProvider;
            this.cacheTimeout = cacheTimeout;
            this.cache = new Dictionary<string, CachingCurrency>();
        }

        public TimeSpan CacheTimeout
        {
            get { return this.cacheTimeout; }
        }

        public override Currency GetCurrency(string currencyCode)
        {
            CachingCurrency currency;
            if (this.cache.TryGetValue(currencyCode, out currency))
            {
                return currency;
            }

            currency = new CachingCurrency(this.innerProvider.GetCurrency(currencyCode), this.CacheTimeout);
            this.cache.Add(currencyCode, currency);
            return currency;
        }
    }
}
