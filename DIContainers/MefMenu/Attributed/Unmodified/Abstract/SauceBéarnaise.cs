using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Ploeh.Samples.MenuModel;

namespace Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Abstract
{
    [Export(typeof(Ploeh.Samples.MenuModel.SauceBéarnaise))]
    [Export]
    [Export(typeof(IIngredient))]
    public partial class SauceBéarnaise : IIngredient { }

    public partial class SauceBéarnaise : Ploeh.Samples.MenuModel.SauceBéarnaise
    {
    }
}
