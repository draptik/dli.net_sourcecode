using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using System.Configuration;

namespace Ploeh.Samples.DI.AntiPatterns.DirectControl.Newing
{
    public class ProductService
    {
        private readonly ProductRepository repository;

        public ProductService()
        {
            string connectionString =
                ConfigurationManager.ConnectionStrings
                ["CommerceObjectContext"].ConnectionString;

            this.repository =
                new SqlProductRepository(connectionString);
        }

        public IEnumerable<Product> GetFeaturedProducts(IPrincipal user)
        {
            return from p in this.repository.GetFeaturedProducts()
                   select p.ApplyDiscountFor(user);
        }
    }
}
