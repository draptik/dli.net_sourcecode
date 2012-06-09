using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Ploeh.Samples.MenuModel;

namespace Ploeh.Samples.Menu.Mef.Attributed.Lifetime.Shared
{
    [Export(typeof(Ploeh.Samples.MenuModel.OliveOil))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    [Export(typeof(IIngredient))]
    public partial class OliveOil
    {
    }

    public partial class OliveOil : Ploeh.Samples.MenuModel.OliveOil
    {
    }
}
