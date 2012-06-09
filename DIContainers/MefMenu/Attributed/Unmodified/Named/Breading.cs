using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Ploeh.Samples.MenuModel;

namespace Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Named
{
    [Export(typeof(IIngredient))]
    public partial class Breading
    {
    }

    public partial class Breading : Ploeh.Samples.MenuModel.Breading
    {
        [ImportingConstructor]
        public Breading([Import("cutlet", typeof(IIngredient))]IIngredient ingredient)
            : base(ingredient)
        {
        }
    }
}
