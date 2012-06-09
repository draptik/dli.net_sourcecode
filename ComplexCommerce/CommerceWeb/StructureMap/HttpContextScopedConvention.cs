using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StructureMap.Graph;
using StructureMap.Configuration.DSL;

namespace Ploeh.Samples.Commerce.Web.StructureMap
{
    public class HttpContextScopedConvention : IRegistrationConvention
    {
        #region IRegistrationConvention Members

        public void Process(Type type, Registry registry)
        {
            registry.For(type).HttpContextScoped();
        }

        #endregion
    }
}
