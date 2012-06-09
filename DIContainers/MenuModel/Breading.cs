using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.MenuModel
{
    public class Breading : IIngredient
    {
        private readonly IIngredient ingredient;

        public Breading(IIngredient ingredient)
        {
            if (ingredient == null)
            {
                throw new ArgumentNullException("ingredient");
            }

            this.ingredient = ingredient;
        }

        public IIngredient Ingredient
        {
            get { return this.ingredient; }
        }
    }
}
