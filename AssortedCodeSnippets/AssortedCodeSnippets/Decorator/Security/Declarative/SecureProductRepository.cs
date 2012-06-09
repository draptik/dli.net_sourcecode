using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Permissions;

namespace Ploeh.Samples.AssortedCodeSnippets.Decorator.Security.Declarative
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

		[PrincipalPermission(SecurityAction.Demand, Role = "ProductManager")]
		public override void InsertProduct(Product product)
		{
			this.innerRepository.InsertProduct(product);
		}
	}
}
