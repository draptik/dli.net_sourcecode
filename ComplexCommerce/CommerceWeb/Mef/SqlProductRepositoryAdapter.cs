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
    public class SqlProductRepositoryAdapter
    {
        private readonly string connectionString;

        [ImportingConstructor]
        public SqlProductRepositoryAdapter([Import("connectionString")]string connectionString)
        {
            if (connectionString == null)
            {
                throw new ArgumentNullException("connectionString");
            }

            this.connectionString = connectionString;
        }

        [Export]
        public ProductRepository SqlProductRepository
        {
            get { return new SqlProductRepository(this.connectionString); }
        }
    }
}
