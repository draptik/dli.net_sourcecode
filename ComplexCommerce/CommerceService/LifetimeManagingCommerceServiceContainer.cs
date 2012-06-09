using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Ploeh.Samples.Commerce.Data.Sql;
using Ploeh.Samples.Commerce.Domain;

namespace Ploeh.Samples.CommerceService
{
    public partial class LifetimeManagingCommerceServiceContainer :
        ICommerceServiceContainer
    {
        private readonly string connectionString;
        private readonly IContractMapper mapper;

        public LifetimeManagingCommerceServiceContainer()
        {
            this.connectionString =
                ConfigurationManager.ConnectionStrings
                ["CommerceObjectContext"].ConnectionString;

            this.mapper = new ContractMapper();
        }

        public IProductManagementService
            ResolveProductManagementService()
        {
            ProductRepository repository =
                new SqlProductRepository(
                    this.connectionString);
            return new ProductManagementService(
                repository, this.mapper);
        }
    }

    public partial class LifetimeManagingCommerceServiceContainer
    {
        public void Release(object instance)
        {
        }
    }
}
