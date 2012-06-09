using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Dependency.Lifetime.Pooled
{
	public partial class PooledContainer : ICommerceServiceContainer
	{
		private readonly IContractMapper mapper;
		private readonly List<XferProductRepository> free;
		private readonly List<XferProductRepository> used;

		public PooledContainer()
		{
			this.mapper = new ContractMapper();
			this.free = new List<XferProductRepository>();
			this.used = new List<XferProductRepository>();
		}

		public int MaxSize { get; set; }

		public bool HasExcessCapacity
		{
			get
			{
				return this.free.Count + this.used.Count < this.MaxSize;
			}
		}
	}

	public partial class PooledContainer
	{
		public void Release(object instance)
		{
			var service = instance as ProductManagementService;
			if (service == null)
			{
				return;
			}
			var repository = service.Repository
				as XferProductRepository;
			if (repository == null)
			{
				return;
			}
			this.used.Remove(repository);
			this.free.Add(repository);
		}

		public IProductManagementService ResolveProductManagementService()
		{
			XferProductRepository repository = null;
			if (this.free.Count > 0)
			{
				repository = this.free[0];
				this.used.Add(repository);
				this.free.Remove(repository);
			}
			if (repository != null)
			{
				return this.ResolveWith(repository);
			}

			if (!this.HasExcessCapacity)
			{
				throw new InvalidOperationException(
					"The pool is full.");
			}

			repository = new XferProductRepository();
			this.used.Add(repository);

			return this.ResolveWith(repository);
		}

		private IProductManagementService ResolveWith(
			ProductRepository repository)
		{
			return new ProductManagementService(repository,
				this.mapper);
		}
	}
}
