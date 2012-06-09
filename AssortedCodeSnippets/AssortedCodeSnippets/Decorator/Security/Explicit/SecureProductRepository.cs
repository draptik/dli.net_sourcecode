using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Permissions;
using System.Threading;
using System.Security;

namespace Ploeh.Samples.AssortedCodeSnippets.Decorator.Security.Explicit
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
			if (!Thread.CurrentPrincipal.IsInRole("ProductManager"))
			{
				throw new SecurityException();
			}

			this.innerRepository.InsertProduct(product);
		}
	}
}
