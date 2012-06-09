using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Ploeh.Samples.MenuModel;

namespace Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Named
{
    [Export("cutlet", typeof(IIngredient))]
    public partial class VealCutlet : IIngredient { }

    public partial class VealCutlet : Ploeh.Samples.MenuModel.VealCutlet
    {
    }
}
