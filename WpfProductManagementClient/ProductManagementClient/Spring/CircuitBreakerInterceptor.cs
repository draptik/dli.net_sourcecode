using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AopAlliance.Intercept;
using Ploeh.Samples.ProductManagement.WcfAgent;

namespace Ploeh.Samples.ProductManagement.WpfClient.Spring
{
    public class CircuitBreakerInterceptor :
        IMethodInterceptor
    {
        private readonly ICircuitBreaker breaker;

        public CircuitBreakerInterceptor(
            ICircuitBreaker breaker)
        {
            if (breaker == null)
            {
                throw new ArgumentNullException("breaker");
            }

            this.breaker = breaker;
        }

        public object Invoke(IMethodInvocation invocation)
        {
            this.breaker.Guard();
            try
            {
                var result = invocation.Proceed();
                this.breaker.Succeed();
                return result;
            }
            catch (Exception e)
            {
                this.breaker.Trip(e);
                throw;
            }
        }
    }
}
