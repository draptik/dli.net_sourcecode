using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Ploeh.Samples.MenuModel;

namespace Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Named
{
    [Export(typeof(ICourse))]
    [Export("entée1", typeof(ICourse))]
    public partial class LobsterBisque
    {
    }

    public partial class LobsterBisque : Ploeh.Samples.MenuModel.LobsterBisque
    {
    }
}
