using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Dependency.Lifetime
{
    public class ProductManagementService : IProductManagementService
    {
        private readonly ProductRepository repository;
        private readonly IContractMapper mapper;

        public ProductManagementService(ProductRepository repository, IContractMapper mapper)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }
            if (mapper == null)
            {
                throw new ArgumentNullException("mapper");
            }

            this.repository = repository;
            this.mapper = mapper;
        }

        public IContractMapper ContractMapper
        {
            get { return this.mapper; }
        }

        public ProductRepository Repository
        {
            get { return this.repository; }
        }
    }
}
