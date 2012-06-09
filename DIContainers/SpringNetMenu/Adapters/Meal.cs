using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.Samples.MenuModel;

namespace Ploeh.Samples.Menu.SpringNet.Adapters
{
    public class Meal : Ploeh.Samples.MenuModel.Meal
    {
        public Meal(params ICourse[] courses)
            : base(courses)
        {
        }
    }
}
