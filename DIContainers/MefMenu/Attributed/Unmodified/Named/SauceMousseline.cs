using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Ploeh.Samples.MenuModel;

namespace Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Named
{
    [Export("sauce", typeof(IIngredient))]
    public partial class SauceMousseline
    {
    }

    public partial class SauceMousseline : Ploeh.Samples.MenuModel.SauceMousseline
    {
    }
}
