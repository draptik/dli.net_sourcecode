using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap.Graph;
using StructureMap.Configuration.DSL;
using Ploeh.Samples.MenuModel;

namespace Ploeh.Samples.Menu.StructureMap
{
    public class SauceConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            var interfaceType = typeof(IIngredient);
            if (!interfaceType.IsAssignableFrom(type))
            {
                return;
            }
            if (!type.Name.StartsWith("Sauce"))
            {
                return;
            }

            registry.For(interfaceType).Use(type);
        }
    }
}
