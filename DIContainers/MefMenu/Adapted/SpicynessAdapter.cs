using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.Samples.MenuModel;
using System.ComponentModel.Composition;

namespace Ploeh.Samples.Menu.Mef.Adapted
{
    public class SpicynessAdapter
    {
        [Export]
        public Spiciness Spicyness
        {
            get { return Spiciness.Hot; }
        }
    }
}
