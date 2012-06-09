using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.MenuModel
{
    public class Course : ICourse
    {
        private readonly IEnumerable<IIngredient> ingredients;

        public Course(IEnumerable<IIngredient> ingredients)
        {
            if (ingredients == null)
            {
                throw new ArgumentNullException("ingredients");
            }

            this.ingredients = ingredients.ToList();
        }

        public IEnumerable<IIngredient> Ingredients
        {
            get { return this.ingredients; }
        }
    }
}
