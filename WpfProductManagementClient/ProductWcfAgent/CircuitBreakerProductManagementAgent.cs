using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.Samples.ProductManagement.PresentationLogic.Wpf;

namespace Ploeh.Samples.ProductManagement.WcfAgent
{
    // For convenience, this class is bundled with the WCF implementation, but it might just as
    // well have been implemented in a separate library.
    public class CircuitBreakerProductManagementAgent : IProductManagementAgent
    {
        private readonly IProductManagementAgent innerAgent;
        private readonly ICircuitBreaker breaker;

        public CircuitBreakerProductManagementAgent(IProductManagementAgent agent, ICircuitBreaker breaker)
        {
            if (agent == null)
            {
                throw new ArgumentNullException("agent");
            }
            if (breaker == null)
            {
                throw new ArgumentNullException("breaker");
            }        

            this.innerAgent = agent;
            this.breaker = breaker;
        }

        #region IProductManagementAgent Members

        public void DeleteProduct(int productId)
        {
            this.breaker.Guard();
            try
            {
                this.innerAgent.DeleteProduct(productId);
                this.breaker.Succeed();
            }
            catch (Exception e)
            {
                this.breaker.Trip(e);
                throw;
            }
        }

        public void InsertProduct(ProductEditorViewModel product)
        {
            this.breaker.Guard();
            try
            {
                this.innerAgent.InsertProduct(product);
                this.breaker.Succeed();
            }
            catch (Exception e)
            {
                this.breaker.Trip(e);
                throw;
            }
        }

        public IEnumerable<ProductViewModel> SelectAllProducts()
        {
            this.breaker.Guard();
            try
            {
                var products = this.innerAgent.SelectAllProducts();
                this.breaker.Succeed();
                return products;
            }
            catch (Exception e)
            {
                this.breaker.Trip(e);
                throw;
            }
        }

        public void UpdateProduct(ProductEditorViewModel product)
        {
            this.breaker.Guard();
            try
            {
                this.innerAgent.UpdateProduct(product);
                this.breaker.Succeed();
            }
            catch (Exception e)
            {
                this.breaker.Trip(e);
                throw;
            }
        }

        #endregion
    }
}
