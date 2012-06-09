using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Ploeh.Samples.MenuModel;

namespace Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Abstract
{
    [Export(typeof(IMeal))]
    public partial class Meal
    {
    }

    public partial class Meal : Ploeh.Samples.MenuModel.Meal
    {
        [ImportingConstructor]
        public Meal([ImportMany]IEnumerable<ICourse> courses)
            : base(courses)
        {
        }
    }
}
