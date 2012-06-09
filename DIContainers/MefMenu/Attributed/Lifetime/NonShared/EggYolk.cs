using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.Samples.MenuModel;
using System.ComponentModel.Composition;

namespace Ploeh.Samples.Menu.Mef.Attributed.Lifetime.NonShared
{
    [Export(typeof(Ploeh.Samples.MenuModel.EggYolk))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof(IIngredient))]
    public partial class EggYolk
    {
    }

    public partial class EggYolk : Ploeh.Samples.MenuModel.EggYolk
    {
    }
}
