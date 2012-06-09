using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Ploeh.Samples.MenuModel;

namespace Ploeh.Samples.Menu.Autofac
{
    public class IngredientModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var a = typeof(Steak).Assembly;
            builder.RegisterAssemblyTypes(a).As<IIngredient>();
        }
    }
}
