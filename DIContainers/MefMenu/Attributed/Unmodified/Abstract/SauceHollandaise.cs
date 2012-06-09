using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.Samples.MenuModel;
using System.ComponentModel.Composition;

namespace Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Abstract
{
    [Export]
    [Export(typeof(IIngredient))]
    public partial class SauceHollandaise
    {
    }

    public partial class SauceHollandaise : Ploeh.Samples.MenuModel.SauceHollandaise
    {
    }
}
