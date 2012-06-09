using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.Samples.MenuModel;
using System.ComponentModel.Composition;

namespace Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Invalid
{
    [Export(typeof(ICourse))]
    public partial class SauceBéarnaise : IIngredient { }

    public partial class SauceBéarnaise : Ploeh.Samples.MenuModel.SauceBéarnaise
    {
    }
}
