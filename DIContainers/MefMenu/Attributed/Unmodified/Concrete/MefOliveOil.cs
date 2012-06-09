using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.Samples.MenuModel;
using System.ComponentModel.Composition;

namespace Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Concrete
{
    [Export(typeof(OliveOil))]
    [Export(typeof(IIngredient))]
    public class MefOliveOil : OliveOil { }
}
