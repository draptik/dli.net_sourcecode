using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Ploeh.Samples.MenuModel;

namespace Ploeh.Samples.Menu.Mef.Attributed.Sequence.ExplicitPick
{
    [Export(typeof(IMeal))]
    public partial class Meal
    {
    }

    public partial class Meal : Ploeh.Samples.MenuModel.Meal
    {
        [ImportingConstructor]
        public Meal(
            [ImportMany("meal", typeof(ICourse))]
            IEnumerable<ICourse> courses)
            : base(courses)
        {
        }
    }
}
