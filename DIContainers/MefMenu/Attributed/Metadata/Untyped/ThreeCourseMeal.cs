using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.Samples.MenuModel;
using System.ComponentModel.Composition;

namespace Ploeh.Samples.Menu.Mef.Attributed.Metadata.Untyped
{
    [Export(typeof(IMeal))]
    public partial class ThreeCourseMeal
    {
    }

    public partial class ThreeCourseMeal : Ploeh.Samples.MenuModel.ThreeCourseMeal
    {
        [ImportingConstructor]
        public ThreeCourseMeal(
            ICourse entrée,
            ICourse mainCourse,
            ICourse dessert)
            : base(entrée, mainCourse, dessert)
        {
        }
    }
}
