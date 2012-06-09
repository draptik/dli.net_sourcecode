using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Ploeh.Samples.MenuModel;

namespace Ploeh.Samples.Menu.Mef.Attributed.Lifetime.NonShared
{
    [Export(typeof(Ploeh.Samples.MenuModel.Vinegar))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof(IIngredient))]
    public partial class Vinegar
    {
    }

    public partial class Vinegar : Ploeh.Samples.MenuModel.Vinegar
    {
    }
}
