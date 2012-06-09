using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using System.Configuration;

namespace Ploeh.Samples.DI.AntiPatterns.BastardInjection
{
    public class ProductService
    {
        private readonly ProductRepository repository;

        public ProductService()
            : this(ProductService.CreateDefaultRepository())
        {
        }

        public ProductService(ProductRepository repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }

            this.repository = repository;
        }

        private static ProductRepository CreateDefaultRepository()
        {
            string connectionString =
                ConfigurationManager.ConnectionStrings
                ["CommerceObjectContext"].ConnectionString;

            return new SqlProductRepository(connectionString);
        }

        public IEnumerable<Product> GetFeaturedProducts(IPrincipal user)
        {
            return from p in this.repository.GetFeaturedProducts()
                   select p.ApplyDiscountFor(user);
        }
    }
}
