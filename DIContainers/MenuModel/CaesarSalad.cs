using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.MenuModel
{
    public class CaesarSalad : ICourse
    {
        private IIngredient extra;

        public CaesarSalad()
        {
            this.extra = new NullIngredient();
        }

        public virtual IIngredient Extra
        {
            get { return this.extra; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                this.extra = value;
            }
        }
    }
}
