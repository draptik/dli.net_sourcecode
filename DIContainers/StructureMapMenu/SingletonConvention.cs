using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap.Graph;
using StructureMap.Configuration.DSL;

namespace Ploeh.Samples.Menu.StructureMap
{
    public class SingletonConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            registry.For(type).Singleton();
        }
    }
}
