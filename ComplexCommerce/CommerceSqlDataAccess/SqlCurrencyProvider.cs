using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.Samples.Commerce.Domain;

namespace Ploeh.Samples.Commerce.Data.Sql
{
    public class SqlCurrencyProvider : CurrencyProvider
    {
        private readonly string connString;

        public SqlCurrencyProvider(string connString)
        {
            this.connString = connString;
        }

        public override Currency GetCurrency(string currencyCode)
        {
            return new SqlCurrency(currencyCode, this.connString);
        }
    }
}
