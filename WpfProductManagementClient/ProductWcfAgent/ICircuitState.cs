using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.ProductManagement.WcfAgent
{
    public interface ICircuitState
    {
        void Guard();

        ICircuitState NextState();

        void Succeed();

        void Trip(Exception e);
    }
}
