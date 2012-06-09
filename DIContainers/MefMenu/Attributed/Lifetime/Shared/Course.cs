using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.Samples.MenuModel;
using System.ComponentModel.Composition;

namespace Ploeh.Samples.Menu.Mef.Attributed.Lifetime.Shared
{
    [Export(typeof(Ploeh.Samples.MenuModel.Course))]
    [Export(typeof(ICourse))]
    [PartCreationPolicy(CreationPolicy.Shared)]
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
