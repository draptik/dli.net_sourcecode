using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap.Configuration.DSL;
using Ploeh.Samples.MenuModel;

namespace Ploeh.Samples.Menu.StructureMap
{
    public class MenuRegistry : Registry
    {
        public MenuRegistry()
        {
            this.For<ICourse>().Use<Course>();
            this.Scan(s =>
            {
                s.AssemblyContainingType<Steak>();
                s.AddAllTypesOf<IIngredient>();

                s.ExcludeType<Breading>();
            });
        }
    }
}
