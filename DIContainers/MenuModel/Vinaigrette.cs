using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.MenuModel
{
    public class Vinaigrette : IIngredient
    {
        private readonly Vinegar vinegar;
        private readonly OliveOil oil;

        public Vinaigrette(Vinegar vinegar, OliveOil oil)
        {
            if (vinegar == null)
            {
                throw new ArgumentNullException("vinegar");
            }
            if (oil == null)
            {
                throw new ArgumentNullException("oil");
            }

            this.vinegar = vinegar;
            this.oil = oil;
        }

        public OliveOil Oil
        {
            get { return this.oil; }
        }

        public Vinegar Vinegar
        {
            get { return this.vinegar; }
        }
    }
}
