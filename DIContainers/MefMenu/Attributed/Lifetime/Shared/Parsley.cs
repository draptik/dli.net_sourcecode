using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Ploeh.Samples.MenuModel;

namespace Ploeh.Samples.Menu.Mef.Attributed.Lifetime.Shared
{
    [Export(typeof(IIngredient))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Parsley
    {
    }

    public partial class Parsley : Ploeh.Samples.MenuModel.Parsley
    {
    }
}
