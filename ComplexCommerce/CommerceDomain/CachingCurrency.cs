using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Commerce.Domain
{
    public partial class CachingCurrency : Currency
    {
        private readonly Currency innerCurrency;
        private readonly TimeSpan cacheTimeout;

        public CachingCurrency(Currency innerCurrency, TimeSpan cacheTimeout)
        {
            if (innerCurrency == null)
            {
                throw new ArgumentNullException("innerCurrency");
            }

            this.innerCurrency = innerCurrency;
            this.cacheTimeout = cacheTimeout;
            this.cache = new Dictionary<string, CurrencyCacheEntry>();
        }

        public TimeSpan CacheTimeout
        {
            get { return this.cacheTimeout; }
        }

        public override string Code
        {
            get { return this.innerCurrency.Code; }
        }

        private readonly Dictionary<string, CurrencyCacheEntry> cache;

        public override decimal GetExchangeRateFor(string currencyCode)
        {
            CurrencyCacheEntry cacheEntry;
            if ((this.cache.TryGetValue(currencyCode,
                out cacheEntry))
                && (!cacheEntry.IsExpired))
            {
                return cacheEntry.ExchangeRate;
            }

            var exchangeRate =
                this.innerCurrency
                .GetExchangeRateFor(currencyCode);

            var expiration =
                TimeProvider.Current.UtcNow + this.CacheTimeout;
            this.cache[currencyCode] =
                new CurrencyCacheEntry(exchangeRate, expiration);

            return exchangeRate;
        }
    }

    public partial class CachingCurrency
    {
        public override void UpdateExchangeRate(string currencyCode, decimal rate)
        {
            this.innerCurrency.UpdateExchangeRate(currencyCode, rate);
        }
    }
}
