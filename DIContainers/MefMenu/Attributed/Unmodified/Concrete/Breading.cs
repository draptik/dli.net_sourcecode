using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.Samples.MenuModel;
using System.ComponentModel.Composition;

namespace Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Concrete
{
    [Export(typeof(IIngredient))]
    public partial class Breading : IIngredient
    {
    }

    public partial class Breading : Ploeh.Samples.MenuModel.Breading
    {
        [ImportingConstructor]
        public Breading(
            [Import(typeof(VealCutlet))]
            IIngredient ingredient)
            : base(ingredient)
        {
        }
    }
}
