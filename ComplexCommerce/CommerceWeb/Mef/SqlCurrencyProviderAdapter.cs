using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ploeh.Samples.Commerce.Data.Sql;
using System.ComponentModel.Composition;
using Ploeh.Samples.Commerce.Domain;

namespace Ploeh.Samples.Commerce.Web.Mef
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class SqlCurrencyProviderAdapter
    {
        private readonly string connectionString;

        [ImportingConstructor]
        public SqlCurrencyProviderAdapter([Import("connectionString")]string connectionString)
        {
            if (connectionString == null)
            {
                throw new ArgumentNullException("connectionString");
            }

            this.connectionString = connectionString;
        }

        [Export]
        public CurrencyProvider SqlCurrencyProvider
        {
            get { return new SqlCurrencyProvider(this.connectionString); }
        }
    }
}
