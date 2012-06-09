using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ploeh.Samples.Commerce.Data.Sql;
using Ploeh.Samples.Commerce.Domain;
using System.ComponentModel.Composition;

namespace Ploeh.Samples.Commerce.Web.Mef
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class SqlBasketRepositoryAdapter
    {
        private readonly string connectionString;

        [ImportingConstructor]
        public SqlBasketRepositoryAdapter([Import("connectionString")]string connectionString)
        {
            if (connectionString == null)
            {
                throw new ArgumentNullException("connectionString");
            }

            this.connectionString = connectionString;
        }

        [Export]
        public BasketRepository SqlBasketRepository
        {
            get { return new SqlBasketRepository(this.connectionString); }
        }
    }
}
