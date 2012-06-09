using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.Samples.MenuModel;
using System.ComponentModel.Composition;

namespace Ploeh.Samples.Menu.Mef.Adapted
{
    public class JunkFoodAdapter
    {
        private readonly IMeal junk;

        public JunkFoodAdapter()
        {
            this.junk = JunkFoodFactory.Create("chicken meal");
        }

        [Export]
        public IMeal JunkFood
        {
            get { return this.junk; }
        }
    }
}
