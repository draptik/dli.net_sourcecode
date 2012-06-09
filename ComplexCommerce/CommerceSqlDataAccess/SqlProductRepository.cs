using System;
using System.Collections.Generic;
using System.Linq;

namespace Ploeh.Samples.Commerce.Data.Sql
{
    public partial class SqlProductRepository : Domain.ProductRepository
    {
        private readonly CommerceObjectContext context;

        public SqlProductRepository(string connString)
        {
            this.context =
                new CommerceObjectContext(connString);
        }

        public override IEnumerable<Domain.Product> GetFeaturedProducts()
        {
            var products = (from p in this.context.Products
                            where p.IsFeatured
                            select p).AsEnumerable();
            return from p in products
                   select p.ToDomainProduct();
        }
    }

    public partial class SqlProductRepository
    {
        public override void DeleteProduct(int id)
        {
            var product = (from p in this.context.Products
                           where p.ProductId == id
                           select p).First();
            this.context.DeleteObject(product);
            this.context.SaveChanges();
        }

        public override void InsertProduct(Domain.Product product)
        {
            var isFeatured = false;
            this.InsertProduct(product, isFeatured);
        }

        public override Domain.Product SelectProduct(int id)
        {
            return (from p in this.context.Products
                    where p.ProductId == id
                    select p).First().ToDomainProduct();
        }

        public override IEnumerable<Domain.Product> SelectAllProducts()
        {
            return this.context.Products.AsEnumerable().Select(p => p.ToDomainProduct());
        }

        public override void UpdateProduct(Domain.Product product)
        {
            var baseCurrency = this.CreateBaseCurrency();

            var localProduct = (from p in this.context.Products
                                where p.ProductId == product.Id
                                select p).First();
            localProduct.Name = product.Name;
            localProduct.UnitPrice = product.UnitPrice.ConvertTo(baseCurrency).Amount;

            this.context.SaveChanges();
        }

        private SqlCurrency CreateBaseCurrency()
        {
            var danishCurrency = new SqlCurrency("DKK", this.context.Connection.ConnectionString);
            return danishCurrency;
        }

        private void InsertProduct(Domain.Product product, bool isFeatured)
        {
            var baseCurrency = this.CreateBaseCurrency();

            var p = new Sql.Product();
            p.IsFeatured = isFeatured;
            p.Name = product.Name;
            p.UnitPrice = product.UnitPrice.ConvertTo(baseCurrency).Amount;

            this.context.AddToProducts(p);
            this.context.SaveChanges();
        }
    }

    public partial class SqlProductRepository : IDisposable
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
                this.context.Dispose();
            }
        }
    }

}
