using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Core.Interceptor;
using Ploeh.Samples.ProductManagement.WcfAgent;
using Castle.DynamicProxy;

namespace Ploeh.Samples.ProductManagement.WpfClient.Windsor
{
    public class CircuitBreakerInterceptor : IInterceptor
    {
        private readonly ICircuitBreaker breaker;

        public CircuitBreakerInterceptor(
            ICircuitBreaker breaker)
        {
            if (breaker == null)
            {
                throw new ArgumentNullException(
                    "breaker");
            }

            this.breaker = breaker;
        }

        public void Intercept(IInvocation invocation)
        {
            this.breaker.Guard();
            try
            {
                invocation.Proceed();
                this.breaker.Succeed();
            }
            catch (Exception e)
            {
                this.breaker.Trip(e);
                throw;
            }
        }
    }
}
