using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Ploeh.Samples.DI.AntiPatterns.DirectControl.Factory.ConcreteFactory
{
    public class ProductRepositoryFactory
    {
        public ProductRepository Create()
        {
            string connectionString =
                ConfigurationManager.ConnectionStrings
                ["CommerceObjectContext"].ConnectionString;
            return new SqlProductRepository(connectionString);
        }
    }
}
