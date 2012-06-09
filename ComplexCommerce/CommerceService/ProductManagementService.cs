using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.Samples.Commerce.Domain;

namespace Ploeh.Samples.CommerceService
{
    public class ProductManagementService : IProductManagementService
    {
        private readonly ProductRepository repository;
        private readonly IContractMapper mapper;

        public ProductManagementService(ProductRepository repository,
            IContractMapper mapper)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }
            if (mapper == null)
            {
                throw new ArgumentNullException("mapper");
            }

            this.repository = repository;
            this.mapper = mapper;
        }

        public IContractMapper Mapper
        {
            get { return this.mapper; }
        }

        public ProductRepository ProductRepository
        {
            get { return this.repository; }
        }

        #region IProductManagementService Members

        public void DeleteProduct(int productId)
        {
            this.repository.DeleteProduct(productId);
        }

        public void InsertProduct(ProductContract product)
        {
            if (product == null)
            {
                throw new ArgumentNullException("product");
            }

            var p = this.mapper.Map(product);
            this.repository.InsertProduct(p);
        }

        public ProductContract SelectProduct(int productId)
        {
            var product = this.repository.SelectProduct(productId);
            return this.mapper.Map(product);
        }

        public ProductContract[] SelectAllProducts()
        {
            var products = this.repository.SelectAllProducts();
            return this.mapper.Map(products).ToArray();
        }

        public void UpdateProduct(ProductContract product)
        {
            if (product == null)
            {
                throw new ArgumentNullException("product");
            }

            var p = this.mapper.Map(product);
            this.repository.UpdateProduct(p);
        }

        #endregion
    }
}
