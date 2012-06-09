using System.Collections.Generic;

namespace Ploeh.Samples.Commerce.Domain
{
    public abstract partial class ProductRepository
    {
        public abstract IEnumerable<Product> GetFeaturedProducts();
    }

    public abstract partial class ProductRepository
    {
        public abstract void DeleteProduct(int id);

        public abstract void InsertProduct(Product product);

        public abstract Product SelectProduct(int id);

        public abstract IEnumerable<Product> SelectAllProducts();

        public abstract void UpdateProduct(Product product);
    }
}
