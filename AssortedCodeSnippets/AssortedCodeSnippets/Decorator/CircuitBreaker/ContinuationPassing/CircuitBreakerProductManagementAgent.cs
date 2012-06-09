using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.AssortedCodeSnippets.Decorator.CircuitBreaker.ContinuationPassing
{
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

        public void InsertProduct(ProductEditorViewModel product)
        {
            this.breaker.Execute(() => 
                this.innerAgent.InsertProduct(product));
        }

        #endregion
    }
}
