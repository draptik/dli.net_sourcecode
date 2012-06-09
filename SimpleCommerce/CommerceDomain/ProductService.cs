using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;

namespace Ploeh.Samples.Commerce.Domain
{
    public class ProductService
    {
        private readonly ProductRepository repository;

        public ProductService(ProductRepository repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }

            this.repository = repository;
        }

        public IEnumerable<DiscountedProduct>
            GetFeaturedProducts(IPrincipal user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return from p in
                       this.repository.GetFeaturedProducts()
                   select p.ApplyDiscountFor(user);
        }
    }
}
