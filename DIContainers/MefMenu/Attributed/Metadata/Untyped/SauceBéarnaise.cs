using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Ploeh.Samples.MenuModel;

namespace Ploeh.Samples.Menu.Mef.Attributed.Metadata.Untyped
{
    [Export(typeof(Ploeh.Samples.MenuModel.SauceBéarnaise))]
    [Export]
    [ExportMetadata("category", "sauce")]
    [Export(typeof(IIngredient))]
    public partial class SauceBéarnaise
    {
    }

    public partial class SauceBéarnaise : Ploeh.Samples.MenuModel.SauceBéarnaise
    {
    }
}
