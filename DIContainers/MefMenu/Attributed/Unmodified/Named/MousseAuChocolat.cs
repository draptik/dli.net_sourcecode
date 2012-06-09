using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Ploeh.Samples.MenuModel;

namespace Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Named
{
    [Export(typeof(ICourse))]
    [Export("dessert", typeof(ICourse))]
    public partial class MousseAuChocolat : ICourse { }

    public partial class MousseAuChocolat : Ploeh.Samples.MenuModel.MousseAuChocolat
    {
    }
}
