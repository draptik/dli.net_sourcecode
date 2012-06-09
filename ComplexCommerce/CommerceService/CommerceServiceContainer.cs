using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.Samples.Commerce.Data.Sql;
using System.Configuration;
using Ploeh.Samples.Commerce.Domain;

namespace Ploeh.Samples.CommerceService
{
    public class CommerceServiceContainer : ICommerceServiceContainer
    {
        public IProductManagementService ResolveProductManagementService()
        {
            string connectionString =
                ConfigurationManager.ConnectionStrings
                ["CommerceObjectContext"].ConnectionString;

            ProductRepository repository =
                new SqlProductRepository(connectionString);

            IContractMapper mapper = new ContractMapper();

            return new ProductManagementService(repository, 
                mapper);
        }

        public void Release(object instance)
        {
        }
    }
}
