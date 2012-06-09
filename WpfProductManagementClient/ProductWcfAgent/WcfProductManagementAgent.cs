using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.Samples.ProductManagement.PresentationLogic.Wpf;

namespace Ploeh.Samples.ProductManagement.WcfAgent
{
    public class WcfProductManagementAgent : IProductManagementAgent
    {
        private readonly IProductChannelFactory factory;
        private readonly IClientContractMapper mapper;

        public WcfProductManagementAgent(IProductChannelFactory factory, IClientContractMapper mapper)
        {
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }
            if (mapper == null)
            {
                throw new ArgumentNullException("mapper");
            }

            this.factory = factory;
            this.mapper = mapper;
        }

        #region IProductManagementAgent Members

        public void DeleteProduct(int productId)
        {
            using (var channel = this.factory.CreateChannel())
            {
                channel.DeleteProduct(productId);
            }
        }

        public void InsertProduct(ProductEditorViewModel product)
        {
            if (product == null)
            {
                throw new ArgumentNullException("product");
            }

            using (var channel = this.factory.CreateChannel())
            {
                var pc = this.mapper.Map(product);
                channel.InsertProduct(pc);
            }
        }

        public IEnumerable<ProductViewModel> SelectAllProducts()
        {
            using (var channel = this.factory.CreateChannel())
            {
                var products = channel.SelectAllProducts();
                return this.mapper.Map(products);
            }
        }

        public void UpdateProduct(ProductEditorViewModel product)
        {
            if (product == null)
            {
                throw new ArgumentNullException("product");
            }

            using (var channel = this.factory.CreateChannel())
            {
                var pc = this.mapper.Map(product);
                channel.UpdateProduct(pc);
            }
        }

        #endregion
    }
}
