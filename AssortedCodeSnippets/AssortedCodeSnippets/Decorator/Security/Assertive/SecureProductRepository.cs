using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Permissions;

namespace Ploeh.Samples.AssortedCodeSnippets.Decorator.Security.Assertive
{
	public class SecureProductRepository : ProductRepository
	{
		private readonly ProductRepository innerRepository;

		public SecureProductRepository(ProductRepository repository)
		{
			if (repository == null)
			{
				throw new ArgumentNullException("repository");
			}

			this.innerRepository = repository;
		}

		public override void InsertProduct(Product product)
		{
			new PrincipalPermission(null, "ProductManager").Demand();

			this.innerRepository.InsertProduct(product);
		}
	}
}
