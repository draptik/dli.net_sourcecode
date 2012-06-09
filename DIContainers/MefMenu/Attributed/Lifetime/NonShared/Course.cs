using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Ploeh.Samples.MenuModel;

namespace Ploeh.Samples.Menu.Mef.Attributed.Lifetime.NonShared
{
    [Export(typeof(Ploeh.Samples.MenuModel.Course))]
    [Export(typeof(ICourse))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class Course
    {
    }

    public partial class Course : Ploeh.Samples.MenuModel.Course
    {
        [ImportingConstructor]
        public Course([ImportMany]IEnumerable<IIngredient> ingredients)
            : base(ingredients)
        {
        }
    }
}
