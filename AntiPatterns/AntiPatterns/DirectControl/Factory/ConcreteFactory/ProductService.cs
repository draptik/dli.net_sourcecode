using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;

namespace Ploeh.Samples.DI.AntiPatterns.DirectControl.Factory.ConcreteFactory
{
    public class ProductService
    {
        private readonly ProductRepository repository;

        public ProductService()
        {
            var factory = new ProductRepositoryFactory();
            this.repository = factory.Create();
        }

        public IEnumerable<Product> GetFeaturedProducts(IPrincipal user)
        {
            return from p in this.repository.GetFeaturedProducts()
                   select p.ApplyDiscountFor(user);
        }
    }
}
