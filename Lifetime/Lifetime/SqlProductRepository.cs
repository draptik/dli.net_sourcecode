using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Dependency.Lifetime
{
    public class SqlProductRepository : ProductRepository
    {
        private readonly string connString;

        public SqlProductRepository(string connString)
        {
            if (connString == null)
            {
                throw new ArgumentNullException("connString");
            }

            this.connString = connString;
        }

        public string ConnectionString
        {
            get { return this.connString; }
        }
    }
}
