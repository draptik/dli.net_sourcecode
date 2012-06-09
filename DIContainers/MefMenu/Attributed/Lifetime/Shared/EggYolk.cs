using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.Samples.MenuModel;
using System.ComponentModel.Composition;

namespace Ploeh.Samples.Menu.Mef.Attributed.Lifetime.Shared
{
    [Export(typeof(Ploeh.Samples.MenuModel.EggYolk))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    [Export(typeof(IIngredient))]
    public partial class EggYolk
    {
    }

    public partial class EggYolk : Ploeh.Samples.MenuModel.EggYolk
    {
    }
}
