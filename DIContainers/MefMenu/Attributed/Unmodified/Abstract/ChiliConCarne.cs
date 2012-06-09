using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.Samples.MenuModel;
using System.ComponentModel.Composition;

namespace Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Abstract
{
    [Export(typeof(ICourse))]
    public partial class ChiliConCarne
    {
    }

    public partial class ChiliConCarne : Ploeh.Samples.MenuModel.ChiliConCarne
    {
        [ImportingConstructor]
        public ChiliConCarne(Spiciness spicyness)
            : base(spicyness)
        {
        }
    }
}
