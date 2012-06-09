using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Ploeh.Samples.MenuModel;

namespace Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Named
{
    [Export(typeof(IMeal))]
    public partial class ThreeCourseMeal
    {
    }

    public partial class ThreeCourseMeal : Ploeh.Samples.MenuModel.ThreeCourseMeal
    {
        [ImportingConstructor]
        public ThreeCourseMeal(
            [Import("entrée", typeof(ICourse))]ICourse entrée,
            [Import("mainCourse", typeof(ICourse))]ICourse mainCourse,
            [Import("dessert", typeof(ICourse))]ICourse dessert)
            : base(entrée, mainCourse, dessert)
        {
        }
    }
}
