using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.Samples.Mary.ECommerce.Data.Sql;

namespace Ploeh.Samples.Mary.ECommerce.Domain
{
    public partial class ProductService : Ploeh.Samples.Mary.ECommerce.Domain.IProductService
    {
        private readonly CommerceObjectContext objectContext;

        public ProductService()
        {
            this.objectContext = new CommerceObjectContext();
        }

        public IEnumerable<Product> GetFeaturedProducts(
            bool isCustomerPreferred)
        {
            var discount = isCustomerPreferred ? .95m : 1;
            var products = (from p in this.objectContext
                                .Products
                            where p.IsFeatured
                            select p).AsEnumerable();
            return from p in products
                   select new Product
                   {
                       ProductId = p.ProductId,
                       Name = p.Name,
                       Description = p.Description,
                       IsFeatured = p.IsFeatured,
                       UnitPrice = p.UnitPrice * discount
                   };
        }
    }

    public partial class ProductService : IDisposable
    {
        #region IDisposable Members

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.objectContext.Dispose();
            }
        }
    }
}
