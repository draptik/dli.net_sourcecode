using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.ProductManagement.WcfAgent
{
    public interface ICircuitBreaker
    {
        void Guard();

        void Trip(Exception e);

        void Succeed();
    }
}
