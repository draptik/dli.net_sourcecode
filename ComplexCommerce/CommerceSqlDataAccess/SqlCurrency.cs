using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.Samples.Commerce.Domain;

namespace Ploeh.Samples.Commerce.Data.Sql
{
    public partial class SqlCurrency : Currency
    {
        private readonly string code;
        private readonly CommerceObjectContext context;

        public SqlCurrency(string currencyCode, string connString)
        {
            this.code = currencyCode;
            this.context = new CommerceObjectContext(connString);
        }

        public override string Code
        {
            get { return this.code; }
        }

        public override decimal GetExchangeRateFor(string currencyCode)
        {
            var rates = (from r in this.context.ExchangeRates
                         where r.CurrencyCode == currencyCode
                         || r.CurrencyCode == this.code
                         select r)
                         .ToDictionary(r => r.CurrencyCode);

            return rates[currencyCode].Rate 
                / rates[this.code].Rate;
        }
    }

    public partial class SqlCurrency
    {
        public override void UpdateExchangeRate(string currencyCode, decimal rate)
        {
            var rates = (from r in this.context.ExchangeRates
                         where r.CurrencyCode == currencyCode
                         || r.CurrencyCode == this.code
                         || r.CurrencyCode == "DKK"
                         select r)
                         .ToDictionary(r => r.CurrencyCode);

            rates[currencyCode].Rate = rate * rates[this.code].Rate;
            this.context.SaveChanges();
        }
    }
}
