using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Ploeh.Samples.MenuModel;

namespace Ploeh.Samples.Menu.Unity
{
    public class IngredientExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            var a = typeof(Steak).Assembly;
            foreach (var t in a.GetExportedTypes())
            {
                // This filter clutters the sample code and is removed in the book
                if (typeof(IIngredient) == t || typeof(Breading) == t)
                {
                    continue;
                }
                if (typeof(IIngredient).IsAssignableFrom(t))
                {
                    this.Container.RegisterType(
                        typeof(IIngredient), t, t.FullName);
                }
            }
        }
    }
}
