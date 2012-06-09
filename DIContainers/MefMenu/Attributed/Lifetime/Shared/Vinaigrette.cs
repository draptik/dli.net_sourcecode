using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Ploeh.Samples.MenuModel;

namespace Ploeh.Samples.Menu.Mef.Attributed.Lifetime.Shared
{
    [Export(typeof(Ploeh.Samples.MenuModel.Vinaigrette))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    [Export(typeof(IIngredient))]
    public partial class Vinaigrette
    {
    }

    public partial class Vinaigrette : Ploeh.Samples.MenuModel.Vinaigrette
    {
        [ImportingConstructor]
        public Vinaigrette(Ploeh.Samples.MenuModel.Vinegar vinegar, Ploeh.Samples.MenuModel.OliveOil oil)
            : base(vinegar, oil)
        {
        }
    }
}
