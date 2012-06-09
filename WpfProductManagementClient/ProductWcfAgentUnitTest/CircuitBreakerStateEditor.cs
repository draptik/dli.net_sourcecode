using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.ProductManagement.WcfAgent.UnitTest
{
    internal static class CircuitBreakerStateEditor
    {
        internal static void PutInOpenState(this CircuitBreaker breaker)
        {
            breaker.Trip(new Exception());
        }

        internal static void PutInHalfOpenState(this CircuitBreaker breaker)
        {
            DateTime.Now.Freeze();
            breaker.PutInOpenState();
            2.Minutes().Pass();
            breaker.Guard();
        }
    }
}
