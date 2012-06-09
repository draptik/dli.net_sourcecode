using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StructureMap.Graph;
using StructureMap.Configuration.DSL;

namespace Ploeh.Samples.Commerce.Web.StructureMap
{
    public class SingletonConvention : IRegistrationConvention
    {
        #region IRegistrationConvention Members

        public void Process(Type type, Registry registry)
        {
            registry.For(type).Singleton();
        }

        #endregion
    }
}
