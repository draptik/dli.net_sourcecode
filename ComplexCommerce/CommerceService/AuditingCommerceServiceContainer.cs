using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Ploeh.Samples.Commerce.Domain;
using Ploeh.Samples.Commerce.Data.Sql;

namespace Ploeh.Samples.CommerceService
{
    public class AuditingCommerceServiceContainer : ICommerceServiceContainer
    {
        #region ICommerceServiceContainer Members

        public void Release(object instance)
        {
        }

        public IProductManagementService ResolveProductManagementService()
        {
            string connectionString =
                ConfigurationManager.ConnectionStrings
                ["CommerceObjectContext"].ConnectionString;

            ProductRepository sqlRepository =
                new SqlProductRepository(connectionString);

            IAuditor sqlAuditor =
                new SqlAuditor(connectionString);

            ProductRepository auditingRepository =
                new AuditingProductRepository(
                    sqlRepository, sqlAuditor);

            IContractMapper mapper = new ContractMapper();

            return new ProductManagementService(
                auditingRepository, mapper);
        }

        #endregion
    }
}
