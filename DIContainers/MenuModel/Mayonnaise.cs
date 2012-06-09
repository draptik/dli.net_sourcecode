using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.MenuModel
{
    public class Mayonnaise : IIngredient
    {
        private readonly EggYolk eggYolk;
        private readonly OliveOil oil;

        public Mayonnaise(EggYolk eggYolk, OliveOil oil)
        {
            if (eggYolk == null)
            {
                throw new ArgumentNullException("eggYolk");
            }
            if (oil == null)
            {
                throw new ArgumentNullException("oil");
            }

            this.eggYolk = eggYolk;
            this.oil = oil;
        }

        public EggYolk EggYolk
        {
            get { return this.eggYolk; }
        }

        public OliveOil Oil
        {
            get { return this.oil; }
        }
    }
}
