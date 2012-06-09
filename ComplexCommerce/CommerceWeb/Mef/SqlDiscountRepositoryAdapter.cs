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
    public class SqlDiscountRepositoryAdapter
    {
        private readonly string connectionString;

        [ImportingConstructor]
        public SqlDiscountRepositoryAdapter([Import("connectionString")]string connectionString)
        {
            if (connectionString == null)
            {
                throw new ArgumentNullException("connectionString");
            }

            this.connectionString = connectionString;
        }

        [Export]
        public DiscountRepository DiscountRepository
        {
            get { return new SqlDiscountRepository(this.connectionString); }
        }
    }
}
