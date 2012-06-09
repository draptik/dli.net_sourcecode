using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Ploeh.Samples.MenuModel;

namespace Ploeh.Samples.Menu.Mef.Adapted.Lifetime.Shared
{
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class MayonnaiseAdapter : Ploeh.Samples.Menu.Mef.Adapted.MayonnaiseAdapter
    {
        [ImportingConstructor]
        public MayonnaiseAdapter(EggYolk eggYolk, OliveOil oil)
            : base(eggYolk, oil)
        {
        }

        [Export]
        public override MenuModel.Mayonnaise Mayonnaise
        {
            get { return base.Mayonnaise; }
        }
    }
}
