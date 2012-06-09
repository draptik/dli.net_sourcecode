using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Ploeh.Samples.DI.AntiPatterns.DirectControl.Factory.StaticFactory.Complex
{
    public static class ProductRepositoryFactory
    {
        public static ProductRepository Create()
        {
            var repositoryType = 
                ConfigurationManager.AppSettings["productRepository"];
            switch (repositoryType)
            {
                case "sql":
                    return ProductRepositoryFactory.CreateSql();
                case "azure":
                    return ProductRepositoryFactory.CreateAzure();
                default:
                    throw new InvalidOperationException("...");
            }            
        }

        private static ProductRepository CreateAzure()
        {
            return new AzureProductRepository();
        }

        private static ProductRepository CreateSql()
        {
            string connectionString =
                ConfigurationManager.ConnectionStrings
                ["CommerceObjectContext"].ConnectionString;
            return new SqlProductRepository(connectionString);
        }
    }
}
