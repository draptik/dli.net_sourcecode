using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.Samples.MenuModel;
using System.ComponentModel.Composition;

namespace Ploeh.Samples.Menu.Mef.Adapted
{
    public class EggYolkAdapter
    {
        private readonly EggYolk eggYolk;

        public EggYolkAdapter()
        {
            this.eggYolk = new EggYolk();
        }

        [Export]
        public virtual EggYolk EggYolk
        {
            get { return this.eggYolk; }
        }
    }
}
