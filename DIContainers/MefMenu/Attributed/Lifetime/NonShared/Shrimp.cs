using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Ploeh.Samples.MenuModel;

namespace Ploeh.Samples.Menu.Mef.Attributed.Lifetime.NonShared
{
    [Export(typeof(Ploeh.Samples.MenuModel.Shrimp))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof(IIngredient))]
    public partial class Shrimp
    {
    }

    public partial class Shrimp : Ploeh.Samples.MenuModel.Shrimp
    {
    }
}
