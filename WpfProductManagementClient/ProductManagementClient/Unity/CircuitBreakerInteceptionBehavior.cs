using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity.InterceptionExtension;
using Ploeh.Samples.ProductManagement.WcfAgent;

namespace Ploeh.Samples.ProductManagement.WpfClient.Unity
{
    public class CircuitBreakerInteceptionBehavior :
        IInterceptionBehavior
    {
        private readonly ICircuitBreaker breaker;

        public CircuitBreakerInteceptionBehavior(
            ICircuitBreaker breaker)
        {
            if (breaker == null)
            {
                throw new ArgumentNullException("breaker");
            }

            this.breaker = breaker;
        }

        public IMethodReturn Invoke(IMethodInvocation input,
            GetNextInterceptionBehaviorDelegate getNext)
        {
            try
            {
                this.breaker.Guard();
            }
            catch (InvalidOperationException e)
            {
                return
                    input.CreateExceptionMethodReturn(e);
            }

            var result = getNext()(input, getNext);
            if (result.Exception != null)
            {
                this.breaker.Trip(result.Exception);
            }
            else
            {
                this.breaker.Succeed();
            }
            return result;
        }

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        public bool WillExecute
        {
            get { return true; }
        }
    }
}
