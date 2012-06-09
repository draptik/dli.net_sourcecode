using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.MenuModel
{
    public class ChiliConCarne : ICourse
    {
        private readonly Spiciness spicyness;

        public ChiliConCarne(Spiciness spicyness)
        {
            this.spicyness = spicyness;
        }

        public Spiciness Spicyness
        {
            get { return this.spicyness; }
        }
    }
}
