using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Ploeh.Samples.MenuModel;

namespace Ploeh.Samples.Menu.Mef.Attributed.Lifetime.NonSharedImport
{
    [Export(typeof(Ploeh.Samples.MenuModel.Mayonnaise))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class Mayonnaise
    {
    }

    public partial class Mayonnaise : Ploeh.Samples.MenuModel.Mayonnaise
    {
        [ImportingConstructor]
        public Mayonnaise(
            [Import(RequiredCreationPolicy = CreationPolicy.NonShared)]
            EggYolk eggYolk,
            OliveOil oil)
            : base(eggYolk, oil)
        { }
    }
}
