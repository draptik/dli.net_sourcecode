using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.Samples.MenuModel;
using System.ComponentModel.Composition;

namespace Ploeh.Samples.Menu.Mef.Attributed.Lifetime.NonShared
{
    [Export(typeof(Ploeh.Samples.MenuModel.Mayonnaise))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof(IIngredient))]
    public partial class Mayonnaise
    {
    }

    public partial class Mayonnaise : Ploeh.Samples.MenuModel.Mayonnaise
    {
        [ImportingConstructor]
        public Mayonnaise(Ploeh.Samples.MenuModel.EggYolk eggYolk, Ploeh.Samples.MenuModel.OliveOil oil)
            : base(eggYolk, oil)
        {
        }
    }
}
