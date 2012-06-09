using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Ploeh.Samples.MenuModel;

namespace Ploeh.Samples.Menu.Mef.Attributed.Sequence.ExplicitPick
{
    [Export(typeof(ICourse))]
    public partial class LobsterBisque { }

    public partial class LobsterBisque : Ploeh.Samples.MenuModel.LobsterBisque
    {
    }
}
