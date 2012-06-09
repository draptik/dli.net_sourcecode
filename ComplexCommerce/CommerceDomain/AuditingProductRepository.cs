using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Commerce.Domain
{
    public partial class AuditingProductRepository : 
        ProductRepository
    {
        private readonly ProductRepository
            innerRepository;
        private readonly IAuditor auditor;

        public AuditingProductRepository(
            ProductRepository repository,
            IAuditor auditor)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }
            if (auditor == null)
            {
                throw new ArgumentNullException("auditor");
            }        

            this.innerRepository = repository;
            this.auditor = auditor;
        }
    }

    public partial class AuditingProductRepository
    {
        public override IEnumerable<Product> GetFeaturedProducts()
        {
            return this.innerRepository.GetFeaturedProducts();
        }

        public override void DeleteProduct(int id)
        {
            this.innerRepository.DeleteProduct(id);
            this.auditor.Record(new AuditEvent("ProductDeleted", id));
        }

        public override void InsertProduct(Product product)
        {
            this.innerRepository.InsertProduct(product);
            this.auditor.Record(new AuditEvent("ProductInserted", product));
        }

        public override Product SelectProduct(int id)
        {
            return this.innerRepository.SelectProduct(id);
        }

        public override void UpdateProduct(Product product)
        {
            this.innerRepository.UpdateProduct(product);
            this.auditor.Record(
                new AuditEvent("ProductUpdated", product));
        }

        public override IEnumerable<Product> SelectAllProducts()
        {
            return this.innerRepository.SelectAllProducts();
        }
    }
}
