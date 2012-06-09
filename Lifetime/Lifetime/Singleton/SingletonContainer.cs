using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Dependency.Lifetime.Singleton
{
	public class SingletonContainer : ICommerceServiceContainer
	{
		private readonly ProductRepository repository;
		private readonly IContractMapper mapper;

		public SingletonContainer()
		{
			this.repository =
				new InMemoryProductRepository();
			this.mapper = new ContractMapper();
		}

		public IProductManagementService ResolveProductManagementService()
		{
			return new ProductManagementService(
				this.repository, this.mapper);
		}

		public void Release(object instance) { }
	}
}
