using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Ploeh.Samples.MenuModel;

namespace Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Abstract
{
    [Export(typeof(IIngredient))]
    public partial class Chicken
    {
    }

    public partial class Chicken : Ploeh.Samples.MenuModel.Chicken
    {
    }
}
