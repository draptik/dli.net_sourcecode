using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Ploeh.Samples.Commerce.Data.Sql;

namespace Ploeh.Samples.CommerceService
{
	public class ReleasingCommerceServiceContainer : ICommerceServiceContainer
	{
		private readonly string connectionString;
		private readonly IContractMapper mapper;
		private readonly Dictionary<IProductManagementService, SqlProductRepository> repositories;
		private readonly object syncRoot;

		public ReleasingCommerceServiceContainer()
		{
			this.connectionString =
				ConfigurationManager.ConnectionStrings
				["CommerceObjectContext"].ConnectionString;

			this.mapper = new ContractMapper();

			this.syncRoot = new object();
			this.repositories = new Dictionary<IProductManagementService, SqlProductRepository>();
		}

		#region ICommerceServiceContainer Members

		public void Release(object instance)
		{
			var srvc = instance as IProductManagementService;
			if (srvc == null)
			{
				return;
			}

			lock (this.syncRoot)
			{
				SqlProductRepository repository;
				if (this.repositories.TryGetValue(srvc, out repository))
				{
					repository.Dispose();
					this.repositories.Remove(srvc);
				}
			}
		}

		public IProductManagementService ResolveProductManagementService()
		{
			var repository = new SqlProductRepository(this.connectionString);
			var srvc = new ProductManagementService(repository, this.mapper);

			lock (this.syncRoot)
			{
				this.repositories.Add(srvc, repository);
			}

			return srvc;
		}

		#endregion
	}
}
