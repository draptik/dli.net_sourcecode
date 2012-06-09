using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.MenuModel
{
    public static class JunkFoodFactory
    {
        public static IMeal Create(string name)
        {
            return new JunkFood(name);
        }
    }
}
