using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.Samples.ProductManagement.WcfAgent.WcfClient;

namespace Ploeh.Samples.ProductManagement.WcfAgent
{
    public interface IProductChannelFactory
    {
        IProductManagementServiceChannel CreateChannel();
    }
}
