using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace Ploeh.Samples.Menu.Mef.Adapted.Lifetime.Shared
{
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class EggYolkAdapter : Ploeh.Samples.Menu.Mef.Adapted.EggYolkAdapter
    {
        [Export]
        public override MenuModel.EggYolk EggYolk
        {
            get { return base.EggYolk; }
        }
    }
}
