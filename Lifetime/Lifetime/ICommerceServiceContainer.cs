using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Dependency.Lifetime
{
    public interface ICommerceServiceContainer
    {
        void Release(object instance);

        IProductManagementService ResolveProductManagementService();
    }
}
